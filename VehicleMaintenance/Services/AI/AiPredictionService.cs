using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.AI;
using VehicleMaintenance.Models;
using VehicleMaintenance.Models.AI;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services.AI;

public class AiPredictionService(
    IServiceScopeFactory scopeFactory,
    IGeminiService gemini,
    ILogger<AiPredictionService> logger) : IAiPredictionService
{
    private readonly IServiceScopeFactory  _scopeFactory = scopeFactory;
    private readonly IGeminiService        _gemini       = gemini;
    private readonly ILogger               _logger       = logger;

    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    // ═══════════════════════════════════════════
    // BACKGROUND TRIGGER
    // ═══════════════════════════════════════════

    public void TriggerBackgroundUpdate(int componentId, int vehicleId)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await GenerateComponentPredictionAsync(componentId);
                await GenerateVehicleSuggestionsAsync(vehicleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Background AI update failed for component {ComponentId}, vehicle {VehicleId}.",
                    componentId, vehicleId);
            }
        });
    }

    // ═══════════════════════════════════════════
    // PER-COMPONENT PREDICTION
    // ═══════════════════════════════════════════

    // How long a component AI result stays fresh before a background trigger re-runs it.
    // Manual calls (POST /api/ai/predict/{id}) bypass this — they always run.
    private static readonly TimeSpan ComponentStaleness = TimeSpan.FromHours(24);

    // Minimum gap between vehicle-suggestion runs triggered by the same action cascade
    // (e.g. adding N components to one record fires N triggers — only the first should run).
    private static readonly TimeSpan SuggestionCooldown = TimeSpan.FromMinutes(10);

    public async Task GenerateComponentPredictionAsync(int componentId, bool forceRefresh = false)
    {
        using var scope   = _scopeFactory.CreateScope();
        var context       = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var component = await context.VehicleComponents
            .Include(c => c.Vehicle)
            .FirstOrDefaultAsync(c => c.VehicleComponentId == componentId);

        if (component is null)
        {
            _logger.LogWarning("GenerateComponentPredictionAsync: component {Id} not found.", componentId);
            return;
        }

        // Staleness guard — skip background re-run if AI result is still fresh.
        // forceRefresh=true is used by the manual /api/ai/predict/{id} endpoint.
        if (!forceRefresh &&
            component.AiGeneratedAt.HasValue &&
            component.AiGeneratedAt.Value > DateTime.UtcNow - ComponentStaleness)
        {
            _logger.LogInformation(
                "Skipping component {Id} AI — result is {H:F0}h old (< {Limit}h threshold).",
                componentId,
                (DateTime.UtcNow - component.AiGeneratedAt.Value).TotalHours,
                ComponentStaleness.TotalHours);
            return;
        }

        var history = await context.MaintenanceRecordComponents
            .Where(mrc => mrc.ComponentId == componentId)
            .Include(mrc => mrc.MaintenanceRecord)
            .OrderByDescending(mrc => mrc.MaintenanceRecord.ServiceDate)
            .Take(3)
            .ToListAsync();

        var profile = await context.UserDrivingProfiles
            .FirstOrDefaultAsync(p => p.UserId == component.Vehicle.UserId);

        var health = ComponentHealthCalculator.Compute(component, component.Vehicle.Mileage);

        var historyPoints  = history.Select(h => (km: h.MaintenanceRecord.Mileage ?? 0, date: h.MaintenanceRecord.ServiceDate)).ToList();
        var avgKmPerYear   = ComputeAvgKmPerYear(component.Vehicle.AverageKmPerYear, profile, historyPoints);
        double? avgKmPerMonth = avgKmPerYear.HasValue ? avgKmPerYear.Value / 12.0 : null;

        try
        {
            var prompt = PromptBuilderService.BuildPredictionPrompt(
                component.Vehicle,
                component,
                history,
                profile,
                health,
                avgKmPerMonth);

            var result = await _gemini.AskJsonAsync<AiPredictionResult>(prompt);
            if (result is null)
            {
                _logger.LogWarning("AI returned null result for component {Id} — skipping save.", componentId);
                return;
            }

            // Require at least one meaningful output before saving
            if (result.EstimatedNextServiceDate is null && result.EstimatedRemainingKm is null)
            {
                _logger.LogWarning("AI returned no date or km estimate for component {Id} — skipping save.", componentId);
                return;
            }

            // Enforce confidence caps — never trust the model to self-limit
            var confidence = CalculateConfidence(
                raw:              result.ConfidenceScore ?? 0.30,
                historyCount:     history.Count,
                hasManualSchedule: component.NextServiceRecommendedKm.HasValue ||
                                   component.NextServiceRecommendedDate.HasValue,
                hasProfile:       profile is not null);

            component.AiEstimatedNextServiceDate = result.EstimatedNextServiceDate;
            component.AiEstimatedRemainingKm     = result.EstimatedRemainingKm.HasValue
                                                    ? Math.Max(0, result.EstimatedRemainingKm.Value)
                                                    : null;
            component.AiConfidenceScore          = confidence;
            component.AiRecommendation           = result.Recommendation;
            component.AiReasoning               = result.Reasoning;
            component.AiGeneratedAt             = DateTime.UtcNow;
            if (result.HealthPercent.HasValue)
                component.AiHealthPercent = Math.Clamp(result.HealthPercent.Value, 0, 100);

            await context.SaveChangesAsync();
            _logger.LogInformation("AI prediction generated for component {Id}.", componentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI prediction failed for component {Id}.", componentId);
        }
    }

    // ═══════════════════════════════════════════
    // VEHICLE-LEVEL SUGGESTIONS
    // ═══════════════════════════════════════════

    public async Task GenerateVehicleSuggestionsAsync(int vehicleId, bool forceRefresh = false)
    {
        using var scope = _scopeFactory.CreateScope();
        var context     = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Cooldown guard — when multiple components are added to one record the trigger fires
        // once per component. Only the first run should actually call Gemini; the rest skip.
        if (!forceRefresh)
        {
            var mostRecent = await context.Predictions
                .Where(p => p.VehicleId == vehicleId && p.Status == PredictionStatus.Active)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => (DateTime?)p.CreatedAt)
                .FirstOrDefaultAsync();

            if (mostRecent.HasValue && mostRecent.Value > DateTime.UtcNow - SuggestionCooldown)
            {
                _logger.LogInformation(
                    "Skipping vehicle {Id} suggestions — last run {M:F0} min ago (< {Limit} min cooldown).",
                    vehicleId,
                    (DateTime.UtcNow - mostRecent.Value).TotalMinutes,
                    SuggestionCooldown.TotalMinutes);
                return;
            }
        }

        var vehicle = await context.Vehicles
            .Include(v => v.VehicleComponents)
            .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

        if (vehicle is null)
        {
            _logger.LogWarning("GenerateVehicleSuggestionsAsync: vehicle {Id} not found.", vehicleId);
            return;
        }

        var recentRecords = await context.MaintenanceRecords
            .Where(r => r.VehicleId == vehicleId)
            .OrderByDescending(r => r.ServiceDate)
            .Take(3)
            .Include(r => r.MaintenanceRecordComponents)
            .ToListAsync();

        var profile = await context.UserDrivingProfiles
            .FirstOrDefaultAsync(p => p.UserId == vehicle.UserId);

        // Profile annual km belongs to the prompt itself — pass null so it isn't double-counted here.
        var recordPoints = recentRecords.Select(r => (km: r.Mileage ?? 0, date: r.ServiceDate)).ToList();
        var avgKmPerYear = ComputeAvgKmPerYear(vehicle.AverageKmPerYear, profile: null, recordPoints);

        try
        {
            var prompt = PromptBuilderService.BuildVehicleSuggestionsPrompt(vehicle, recentRecords, profile, avgKmPerYear);
            var suggestions = await _gemini.AskJsonAsync<List<AiSuggestion>>(prompt);

            if (suggestions is null || suggestions.Count == 0) return;

            suggestions = suggestions.Take(5).ToList();

            var old = await context.Predictions
                .Where(p => p.VehicleId == vehicleId && p.Status == PredictionStatus.Active)
                .ToListAsync();
            context.Predictions.RemoveRange(old);

            var savedCount = 0;
            foreach (var s in suggestions)
            {
                if (s.Title is null || s.Description is null || s.Urgency is null)
                {
                    _logger.LogWarning("Skipping suggestion with missing required fields for vehicle {Id}.", vehicleId);
                    continue;
                }

                // The AI returns the component's integer ID directly from the prompt list,
                // or null for vehicle-level suggestions. Validate it belongs to this vehicle
                // so a hallucinated ID can't link to another vehicle's component.
                int? resolvedComponentId = null;
                if (s.VehicleComponentId.HasValue)
                {
                    resolvedComponentId = vehicle.VehicleComponents
                        .Any(c => c.VehicleComponentId == s.VehicleComponentId.Value)
                        ? s.VehicleComponentId
                        : null;
                }

                decimal? confidence = s.ConfidenceScore.HasValue
                    ? (decimal)Math.Min(s.ConfidenceScore.Value, 0.75)
                    : null;

                context.Predictions.Add(new Prediction
                {
                    VehicleId            = vehicleId,
                    VehicleComponentId   = resolvedComponentId,
                    Title                = s.Title,
                    Description          = s.Description,
                    Urgency              = s.Urgency,
                    ConfidenceScore      = confidence,
                    SuggestedByDate      = s.SuggestedByDate,
                    EstimatedRemainingKm = s.EstimatedRemainingKm.HasValue
                                            ? Math.Max(0, s.EstimatedRemainingKm.Value)
                                            : null,
                    Status               = PredictionStatus.Active,
                    CreatedAt            = DateTime.UtcNow
                });
                savedCount++;
            }

            await context.SaveChangesAsync();
            _logger.LogInformation("AI suggestions generated for vehicle {Id}: {Count}/{Total} predictions saved.",
                vehicleId, savedCount, suggestions.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI suggestions failed for vehicle {Id}.", vehicleId);
        }
    }

    // ═══════════════════════════════════════════
    // DIAGNOSIS
    // ═══════════════════════════════════════════

    // Attached in code — not requested from the model (saves tokens, guarantees exact text).
    private const string DiagnosisDisclaimer =
        "This is an AI-assisted assessment only. Always consult a qualified mechanic " +
        "before making repair decisions or continuing to drive if safety may be affected.";

    public async Task<AiDiagnosisDto?> DiagnoseAsync(int vehicleId, string symptom)
    {
        using var scope = _scopeFactory.CreateScope();
        var context     = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var vehicle = await context.Vehicles
            .Include(v => v.VehicleComponents)
            .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

        if (vehicle is null)
        {
            _logger.LogWarning("DiagnoseAsync: vehicle {Id} not found.", vehicleId);
            return null;
        }

        var recentRecords = await context.MaintenanceRecords
            .Where(r => r.VehicleId == vehicleId)
            .OrderByDescending(r => r.ServiceDate)
            .Take(2)
            .Include(r => r.MaintenanceRecordComponents)
                .ThenInclude(mrc => mrc.Component)
            .ToListAsync();

        var profile = await context.UserDrivingProfiles
            .FirstOrDefaultAsync(p => p.UserId == vehicle.UserId);

        var prompt = PromptBuilderService.BuildDiagnosisPrompt(vehicle, recentRecords, profile, symptom);
        var result = await _gemini.AskJsonAsync<AiDiagnosisResult>(prompt);
        if (result is null) return null;

        var causes    = result.LikelyCauses       ?? [];
        var actions   = result.RecommendedActions ?? [];
        var related   = result.RelatedComponents  ?? [];
        var urgency   = result.Urgency            ?? "safe";
        var urgencyEx = result.UrgencyExplanation ?? "Unable to determine urgency from available data.";

        var entity = new AiDiagnosis
        {
            VehicleId              = vehicleId,
            SymptomDescription     = symptom,
            Urgency                = urgency,
            Summary                = urgencyEx,
            LikelyCauses           = JsonSerializer.Serialize(causes),
            RecommendedActions     = JsonSerializer.Serialize(actions),
            RelatedComponentNames  = related.Count > 0 ? JsonSerializer.Serialize(related) : null,
            CreatedAt              = DateTime.UtcNow
        };

        context.AiDiagnoses.Add(entity);
        await context.SaveChangesAsync();

        return MapToDto(entity, causes, actions, related);
    }

    public async Task<List<AiDiagnosisDto>> GetDiagnosisHistoryAsync(int vehicleId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context     = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var diagnoses = await context.AiDiagnoses
            .Where(d => d.VehicleId == vehicleId)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync();

        return diagnoses.Select(d => MapToDto(
            d,
            JsonSerializer.Deserialize<List<string>>(d.LikelyCauses,       _jsonOptions) ?? [],
            JsonSerializer.Deserialize<List<string>>(d.RecommendedActions,  _jsonOptions) ?? [],
            d.RelatedComponentNames is not null
                ? JsonSerializer.Deserialize<List<string>>(d.RelatedComponentNames, _jsonOptions) ?? []
                : []
        )).ToList();
    }

    private static AiDiagnosisDto MapToDto(
        AiDiagnosis entity,
        List<string> causes,
        List<string> actions,
        List<string> related) => new()
    {
        AiDiagnosisId      = entity.AiDiagnosisId,
        Symptom            = entity.SymptomDescription,
        Urgency            = entity.Urgency,
        UrgencyExplanation = entity.Summary,
        LikelyCauses       = causes,
        RecommendedActions = actions,
        RelatedComponents  = related,
        Disclaimer         = DiagnosisDisclaimer,
        CreatedAt          = entity.CreatedAt,
    };

    // ═══════════════════════════════════════════
    // SHARED HELPERS
    // ═══════════════════════════════════════════

    // Caps the raw AI confidence against history depth and adjusts for schedule/profile signals.
    // Extracted so it can be read and tested in isolation.
    private static double CalculateConfidence(
        double raw,
        int    historyCount,
        bool   hasManualSchedule,
        bool   hasProfile)
    {
        var confidence = historyCount switch
        {
            0 => Math.Min(raw, 0.40),
            1 => Math.Min(raw, 0.60),
            2 => Math.Min(raw, 0.75),
            _ => Math.Min(raw, 0.85)
        };
        if (hasManualSchedule) confidence = Math.Min(confidence + 0.10, 0.85);
        if (!hasProfile)       confidence = Math.Max(confidence - 0.10, 0.0);
        return confidence;
    }

    // Priority: 1 vehicle stored avg 2 profile annual km 3 compute from record span.
    // Pass profile=null when the prompt already receives it directly (vehicle suggestions).
    private static int? ComputeAvgKmPerYear(
        int? vehicleStoredAvg,
        UserDrivingProfile? profile,
        IList<(int km, DateTime date)> records)
    {
        if (vehicleStoredAvg > 0) return vehicleStoredAvg;
        if (profile?.AnnualKm > 0) return profile.AnnualKm;
        if (records.Count >= 2)
        {
            var oldest    = records.MinBy(r => r.date)!;
            var newest    = records.MaxBy(r => r.date)!;
            var kmDelta   = newest.km - oldest.km;
            var daysDelta = (newest.date - oldest.date).Days;
            if (daysDelta > 30 && kmDelta > 0)
                return (int)(kmDelta / (daysDelta / 365.25));
        }
        return null;
    }
}
