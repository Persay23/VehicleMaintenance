using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.AI;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Services.AI;
using VehicleMaintenance.Services.Receipts;
using VehicleMaintenance.Services.Storage;

namespace VehicleMaintenance.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiController(
    IAiPredictionService aiPrediction,
    IGeminiService gemini,
    IReceiptParsingService receiptParsing,
    AppDbContext context,
    IMapper mapper) : ControllerBase
{
    private readonly IAiPredictionService  _aiPrediction   = aiPrediction;
    private readonly IGeminiService        _gemini         = gemini;
    private readonly IReceiptParsingService _receiptParsing = receiptParsing;
    private readonly AppDbContext          _context        = context;
    private readonly IMapper               _mapper         = mapper;

    // ═══════════════════════════════════════════
    // PING
    // ═══════════════════════════════════════════

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        try
        {
            var result = await _gemini.AskAsync(
                "Reply with this exact JSON and nothing else: {\"status\": \"ok\", \"message\": \"Gemini is connected\"}");
            return Ok(new { raw = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // ═══════════════════════════════════════════
    // FEATURE 1 — PER-COMPONENT AI PREDICTION
    // ═══════════════════════════════════════════

    /// <summary>
    /// Runs AI prediction for a single component and saves results on VehicleComponent.
    /// Returns the updated VehicleComponentDto.
    /// </summary>
    [HttpPost("predict/{componentId:int}")]
    public async Task<ActionResult<VehicleComponentDto>> PredictComponent(int componentId)
    {
        var accessError = await CheckComponentAccessAsync(componentId);
        if (accessError is not null) return (ActionResult)accessError;

        try
        {
            // forceRefresh: true — user explicitly requested this, bypass staleness guard
            await _aiPrediction.GenerateComponentPredictionAsync(componentId, forceRefresh: true);
            return Ok(await FetchComponentDtoAsync(componentId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"AI prediction failed: {ex.Message}" });
        }
    }

    // ═══════════════════════════════════════════
    // FEATURE 2 — VEHICLE-LEVEL SUGGESTIONS
    // ═══════════════════════════════════════════

    /// <summary>
    /// Generates 3–5 prioritised AI action suggestions for a vehicle.
    /// Replaces existing Active predictions for that vehicle.
    /// Frontend should re-fetch predictions after this call.
    /// </summary>
    [HttpPost("suggest/{vehicleId:int}")]
    public async Task<IActionResult> GenerateVehicleSuggestions(int vehicleId)
    {
        var accessError = await CheckVehicleAccessAsync(vehicleId);
        if (accessError is not null) return accessError;

        try
        {
            // forceRefresh: true — user explicitly requested this, bypass cooldown guard
            await _aiPrediction.GenerateVehicleSuggestionsAsync(vehicleId, forceRefresh: true);
            return Ok(new { message = "Suggestions generated." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"AI suggestions failed: {ex.Message}" });
        }
    }

    // ═══════════════════════════════════════════
    // FEATURE 3 — DIAGNOSIS
    // ═══════════════════════════════════════════

    /// <summary>
    /// Diagnoses a vehicle symptom described by the user.
    /// Returns causes, urgency, recommended actions, and a disclaimer.
    /// Not cached — every call is fresh.
    /// </summary>
    [HttpPost("diagnose/{vehicleId:int}")]
    public async Task<ActionResult<AiDiagnosisDto>> Diagnose(int vehicleId, [FromBody] DiagnoseRequestDto dto)
    {
        var accessError = await CheckVehicleAccessAsync(vehicleId);
        if (accessError is not null) return (ActionResult)accessError;

        try
        {
            var result = await _aiPrediction.DiagnoseAsync(vehicleId, dto.Symptom.Trim());
            if (result is null)
                return StatusCode(500, new { error = "AI did not return a valid response." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Diagnosis failed: {ex.Message}" });
        }
    }

    [HttpGet("diagnose/{vehicleId:int}")]
    public async Task<ActionResult<AiDiagnosisDto[]>> GetDiagnosisHistory(int vehicleId)
    {
        var accessError = await CheckVehicleAccessAsync(vehicleId);
        if (accessError is not null) return (ActionResult)accessError;

        var history = await _aiPrediction.GetDiagnosisHistoryAsync(vehicleId);
        return Ok(history);
    }

    // ═══════════════════════════════════════════
    // FEATURE 4 — DOCUMENT PHOTO PARSING ("Smart Fill")
    // ═══════════════════════════════════════════

    private static readonly string[] _parseTargets = { "record", "fuel", "component", "expense", "vehicle" };

    /// <summary>
    /// Extracts form fields from a photographed document for the given target form.
    /// Nothing is saved — the response pre-fills the form for the user to review and submit.
    /// target ∈ record | fuel | component | expense | vehicle.
    /// </summary>
    [HttpPost("parse/{target}")]
    public async Task<IActionResult> ParseDocument(string target, IFormFile image, CancellationToken ct)
    {
        var key = target.ToLowerInvariant();
        if (!_parseTargets.Contains(key))
            return BadRequest(new { error = $"Unknown parse target '{target}'." });

        var error = ImageUploadValidator.Validate(image);
        if (error is not null) return BadRequest(new { error });

        using var ms = new MemoryStream();
        await image.CopyToAsync(ms, ct);
        var bytes = ms.ToArray();
        var mime  = image.ContentType;

        object? result = key switch
        {
            "record"    => await _receiptParsing.ParseReceiptAsync(bytes, mime, ct),
            "fuel"      => await _receiptParsing.ParseFuelAsync(bytes, mime, ct),
            "component" => await _receiptParsing.ParseComponentAsync(bytes, mime, ct),
            "expense"   => await _receiptParsing.ParseExpenseAsync(bytes, mime, ct),
            "vehicle"   => await _receiptParsing.ParseVehicleAsync(bytes, mime, ct),
            _           => null,
        };

        if (result is null)
            return StatusCode(500, new { error = "Could not read the document. Please enter the details manually." });

        return Ok(result);
    }

    // ═══════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════

    private string? GetCurrentUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    /// <summary>Returns null if the current user owns the vehicle; NotFound or Forbid otherwise.</summary>
    private async Task<IActionResult?> CheckVehicleAccessAsync(int vehicleId)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        if (vehicle is null)    return NotFound(new { error = "Vehicle not found." });
        if (vehicle.UserId != GetCurrentUserId()) return Forbid();
        return null;
    }

    /// <summary>Returns null if the current user owns the component's vehicle; NotFound or Forbid otherwise.</summary>
    private async Task<IActionResult?> CheckComponentAccessAsync(int componentId)
    {
        var component = await _context.VehicleComponents
            .Include(c => c.Vehicle)
            .FirstOrDefaultAsync(c => c.VehicleComponentId == componentId);
        if (component is null)  return NotFound(new { error = "Component not found." });
        if (component.Vehicle.UserId != GetCurrentUserId()) return Forbid();
        return null;
    }

    /// <summary>Re-fetches a component from the controller's context and maps it to a DTO.</summary>
    private async Task<VehicleComponentDto?> FetchComponentDtoAsync(int componentId)
    {
        var component = await _context.VehicleComponents
            .FirstOrDefaultAsync(c => c.VehicleComponentId == componentId);
        return component is null ? null : _mapper.Map<VehicleComponentDto>(component);
    }
}
