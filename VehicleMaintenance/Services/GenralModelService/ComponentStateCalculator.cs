namespace VehicleMaintenance.Services.GenralModelService;

public static class ComponentStateCalculator
{
    /// <summary>
    /// Derives a component state string from remaining-life percentages.
    /// Single source of truth used by both the health service and the AI prompt builder.
    /// </summary>
    public static string DeriveState(int lifetimeKm, int kmUsed, int lifetimeYears, int daysUsed)
    {
        var kmPct = lifetimeKm > 0
            ? Math.Max(0.0, (1.0 - (double)kmUsed / lifetimeKm) * 100)
            : 100.0;
        var yearsPct = lifetimeYears > 0
            ? Math.Max(0.0, (1.0 - daysUsed / (lifetimeYears * 365.25)) * 100)
            : 100.0;

        return Math.Min(kmPct, yearsPct) switch
        {
            <= 15 => "Critical",
            <= 30 => "Repair",
            <= 50 => "Normal",
            <= 75 => "Good",
            _     => "Perfect"
        };
    }
}
