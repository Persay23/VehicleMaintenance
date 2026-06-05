using VehicleMaintenance.Models;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services.AI;

// Prompt changelog:
// v1 — full prediction prompt with wear rules, driving profile, complaint detection

public static partial class PromptBuilderService
{
    // ═══════════════════════════════════════════
    // SHARED HELPERS
    // ═══════════════════════════════════════════

    private static string FormatServiceHistory(List<MaintenanceRecordComponent> history)
    {
        if (history.Count == 0)
            return "No service history recorded for this component. Base estimate on rated lifetime and current usage only. This significantly reduces confidence.";

        return string.Join("\n", history.Select((r, i) => $"""
            Record {i + 1}:
              Date:               {r.MaintenanceRecord.ServiceDate:yyyy-MM-dd}
              Vehicle mileage:    {r.MaintenanceRecord.Mileage} km
              Action:             {r.ComponentChangeType}
              State before:       {r.OldState}
              State after:        {r.NewState}
              Customer complaint: {(string.IsNullOrWhiteSpace(r.CustomerComplaint) ? "None" : r.CustomerComplaint)}
              Work done:          {(string.IsNullOrWhiteSpace(r.WorkDescription) ? "Not recorded" : r.WorkDescription)}
              Parts replaced:     {(string.IsNullOrWhiteSpace(r.ChangedParts) ? "Not recorded" : r.ChangedParts)}
              Mechanic notes:     {(string.IsNullOrWhiteSpace(r.MaintenanceRecord.Notes) ? "None" : r.MaintenanceRecord.Notes)}
            """));
    }

    private static string FormatDrivingProfile(UserDrivingProfile? profile)
    {
        if (profile is null)
            return "No driving profile available. Assume average conditions: Normal style, Mixed use, 15,000 km/year. Note this assumption in reasoning.";

        return $"""
            Driving style:  {profile.DrivingStyle}
            Primary use:    {profile.PrimaryUsage}
            Annual km:      {profile.AnnualKm} km/year
            Usage pattern:  {profile.UsagePattern}
            Climate zone:   {profile.ClimateZone}
            Parking type:   {profile.ParkingType}
            """;
    }

    private static string BuildComponentSummary(Vehicle vehicle, DateTime referenceDate)
    {
        if (vehicle.VehicleComponents is null || vehicle.VehicleComponents.Count == 0)
            return "No components recorded.";

        var today = referenceDate.Date;

        return string.Join("\n", vehicle.VehicleComponents.Select(c =>
        {
            var kmUsed   = Math.Max(0, vehicle.Mileage - c.InstalledAtVehicleMileage);
            var daysOld  = Math.Max(0, (today - c.InstallationDate.Date).Days);
            var yearsOld = daysOld / 365.25;
            var state    = ComponentStateCalculator.DeriveState(c.ExpectedLifetimeKm, kmUsed, c.ExpectedLifetimeYears, daysOld);

            var name  = c.VehicleComponentName ?? c.ComponentType.ToString();
            var brand = !string.IsNullOrWhiteSpace(c.VehicleComponentBrand) ? $" ({c.VehicleComponentBrand})" : "";

            string kmPart;
            if (c.InstalledAtVehicleMileage == 0 && vehicle.Mileage > 0)
            {
                kmPart = c.ExpectedLifetimeKm > 0
                    ? $"km-ref not set / {c.ExpectedLifetimeKm} km rated"
                    : "km-ref not set";
            }
            else if (c.ExpectedLifetimeKm > 0)
            {
                var kmPct     = Math.Min(100.0, kmUsed * 100.0 / c.ExpectedLifetimeKm);
                var remaining = c.AiEstimatedRemainingKm ?? Math.Max(0, c.ExpectedLifetimeKm - kmUsed);
                kmPart = $"{kmUsed}/{c.ExpectedLifetimeKm} km ({kmPct:F0}% used), ~{remaining} km left";
            }
            else
            {
                kmPart = $"{kmUsed} km since install (no km limit set)";
            }

            string agePart;
            if (c.ExpectedLifetimeYears > 0)
            {
                var yearsPct = Math.Min(100.0, yearsOld / c.ExpectedLifetimeYears * 100);
                agePart = $"{yearsOld:F1}/{c.ExpectedLifetimeYears} yrs ({yearsPct:F0}% used)";
            }
            else
            {
                agePart = $"{yearsOld:F1} yrs old (no year limit set)";
            }

            var installMonth  = c.InstallationDate.ToString("yyyy-MM");
            var aiNextService = c.AiEstimatedNextServiceDate.HasValue
                ? $" | AI next: {c.AiEstimatedNextServiceDate:yyyy-MM-dd}"
                : "";

            return $"[#{c.VehicleComponentId}] {name}{brand} — {c.ComponentType} | State: {state} | {kmPart} | installed {installMonth}, {agePart}{aiNextService}";
        }));
    }

    private static string BuildRecentRecordsSummary(List<MaintenanceRecord> records)
    {
        if (records.Count == 0) return "No service records.";

        return string.Join("\n", records.Select(r =>
        {
            var components = r.MaintenanceRecordComponents?.Count > 0
                ? string.Join("; ", r.MaintenanceRecordComponents.Select(mrc =>
                {
                    var work = !string.IsNullOrWhiteSpace(mrc.WorkDescription)
                        ? $": {mrc.WorkDescription}"
                        : "";
                    return mrc.ComponentChangeType.ToString() + work;
                }))
                : "no components listed";

            return $"- {r.ServiceDate:yyyy-MM-dd} @ {r.Mileage} km — {r.ServiceName} | {components}";
        }));
    }
}
