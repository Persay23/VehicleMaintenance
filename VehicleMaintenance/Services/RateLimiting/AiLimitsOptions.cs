namespace VehicleMaintenance.Services.RateLimiting;

public class AiLimitsOptions
{
    public Dictionary<string, TierLimits> Tiers { get; set; } = [];
    public string DefaultTier { get; set; } = "Regular";

    public Dictionary<string, string> UserOverrides { get; set; } = [];
}

public class TierLimits // 0 means "unlimited".
{
    public int DailyLimit { get; set; }
    public int PerMinute { get; set; }
}
