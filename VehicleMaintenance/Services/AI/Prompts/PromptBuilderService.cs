using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.GenralModelService;

namespace VehicleMaintenance.Services.AI;


public static partial class PromptBuilderService
{

    // SHARED HELPERS

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
            var health   = ComponentHealthCalculator.Compute(c, vehicle.Mileage, referenceDate);
            var yearsOld = health.DaysUsed / 365.25;

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
                var kmUsedPct = Math.Min(100.0, 100.0 - health.KmRemainingPercent);
                var remaining = c.AiEstimatedRemainingKm ?? health.RemainingKm;
                kmPart = $"{health.KmUsed}/{c.ExpectedLifetimeKm} km ({kmUsedPct:F0}% used), ~{remaining} km left";
            }
            else
            {
                kmPart = $"{health.KmUsed} km since install (no km limit set)";
            }

            string agePart;
            if (c.ExpectedLifetimeYears > 0)
            {
                var yearsUsedPct = Math.Min(100.0, 100.0 - health.YearsRemainingPercent);
                agePart = $"{yearsOld:F1}/{c.ExpectedLifetimeYears} yrs ({yearsUsedPct:F0}% used)";
            }
            else
            {
                agePart = $"{yearsOld:F1} yrs old (no year limit set)";
            }

            var installMonth  = c.InstallationDate.ToString("yyyy-MM");
            var aiNextService = c.AiEstimatedNextServiceDate.HasValue
                ? $" | AI next: {c.AiEstimatedNextServiceDate:yyyy-MM-dd}"
                : "";

            return $"[#{c.VehicleComponentId}] {name}{brand} — {c.ComponentType} | State: {health.State} | {kmPart} | installed {installMonth}, {agePart}{aiNextService}";
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
