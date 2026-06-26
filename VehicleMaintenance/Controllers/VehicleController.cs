using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Export;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class VehicleController(
        IVehicleService iVehicleService,
        IVehicleExportService exportService,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IVehicleService _iVehicleService = iVehicleService;
        private readonly IVehicleExportService _exportService = exportService;
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<VehicleDto[]>> GetVehicles()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var vehicles = await _iVehicleService.GetAllVehiclesAsync(userId);
            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(int id)
        {
            var vehicle = await _iVehicleService.GetVehicleByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet("{vehicleId}/summary/costs")]
        public async Task<IActionResult> GetCostSummary(int vehicleId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var summary = await _iVehicleService.GetCostSummaryAsync(vehicleId, from, to);
            if (summary is null) return NotFound();
            return Ok(summary);
        }

        [HttpGet("{vehicleId}/summary/timeline")]
        public async Task<IActionResult> GetTimeline(int vehicleId)
        {
            var timeline = await _iVehicleService.GetTimelineAsync(vehicleId);
            return Ok(timeline);
        }

        /// <summary>Exports the vehicle's full service history as Markdown or PDF.</summary>
        [HttpGet("{id:int}/export")]
        public async Task<IActionResult> ExportVehicle(int id, [FromQuery] string format = "md", CancellationToken ct = default)
        {
            var fmt = format.ToLowerInvariant();
            if (fmt is not ("md" or "pdf"))
                return BadRequest(new { error = "format must be 'md' or 'pdf'." });

            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var file = await _exportService.ExportAsync(id, userId, fmt, ct);
            if (file is null) return NotFound();

            return File(file.Content, file.ContentType, file.FileName);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto createVehicleDto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var createdVehicle = await _iVehicleService.CreateVehicleAsync(createVehicleDto, userId);
            return Ok(createdVehicle);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var updated = await _iVehicleService.UpdateVehicleByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var deleted = await _iVehicleService.DeleteVehicleByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
