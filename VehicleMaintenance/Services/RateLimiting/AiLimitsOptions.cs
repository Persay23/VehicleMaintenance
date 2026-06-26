namespace VehicleMaintenance.Services.RateLimiting;

/// <summary>
/// Bound from the "AiLimits" config section. Tier limits are config-driven so a user can be
/// raised to a higher tier (e.g. "Max" = unlimited) without a migration — the override map keys
/// by email. The DB-backed subscription system (persisted User.Tier) is deferred; only the
/// source of the tier name changes when it lands.
/// </summary>
public class AiLimitsOptions
{
    public Dictionary<string, TierLimits> Tiers { get; set; } = new();
    public string DefaultTier { get; set; } = "Regular";

    /// <summary>email → tier name. Lets specific users (testers, you) get a higher tier via config.</summary>
    public Dictionary<string, string> UserOverrides { get; set; } = new();
}

/// <summary>Per-tier limits. A value of 0 means "unlimited".</summary>
public class TierLimits
{
    public int DailyLimit { get; set; }
    public int PerMinute { get; set; }
}
