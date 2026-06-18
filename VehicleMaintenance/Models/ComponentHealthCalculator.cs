using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Models;

/// <summary>
/// All derived measurements for one component at a point in time.
/// Computed once by <see cref="ComponentHealthCalculator.Compute"/> — read everywhere else.
/// </summary>
public record ComponentMeasurements(
    int    KmUsed,
    int    DaysUsed,
    int    RemainingKm,
    double KmRemainingPercent,
    double YearsRemainingPercent,
    string State
);

/// <summary>
/// Single source of truth for component health and mileage calculations.
/// All services, prompt builders, and health endpoints call Compute — no inline arithmetic elsewhere.
/// </summary>
public static class ComponentHealthCalculator
{
    public static ComponentMeasurements Compute(
        VehicleComponent component,
        int              currentMileage,
        DateTime?        referenceDate = null)
    {
        var today = (referenceDate ?? DateTime.UtcNow).Date;

        var kmUsed   = Math.Max(0, currentMileage - component.InstalledAtVehicleMileage);
        var daysUsed = Math.Max(0, (today - component.InstallationDate.Date).Days);

        var kmRemainingPct = component.ExpectedLifetimeKm > 0
            ? Math.Max(0.0, (1.0 - (double)kmUsed / component.ExpectedLifetimeKm) * 100)
            : 100.0;

        var yearsRemainingPct = component.ExpectedLifetimeYears > 0
            ? Math.Max(0.0, (1.0 - daysUsed / (component.ExpectedLifetimeYears * 365.25)) * 100)
            : 100.0;

        return new ComponentMeasurements(
            KmUsed:                kmUsed,
            DaysUsed:              daysUsed,
            RemainingKm:           Math.Max(0, component.ExpectedLifetimeKm - kmUsed),
            KmRemainingPercent:    Math.Round(kmRemainingPct,    1),
            YearsRemainingPercent: Math.Round(yearsRemainingPct, 1),
            State:                 ComponentStateCalculator.DeriveState(
                                       component.ExpectedLifetimeKm, kmUsed,
                                       component.ExpectedLifetimeYears, daysUsed)
        );
    }
}
