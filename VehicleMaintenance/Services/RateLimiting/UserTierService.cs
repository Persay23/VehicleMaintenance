using Microsoft.Extensions.Options;

namespace VehicleMaintenance.Services.RateLimiting;

public class UserTierService(IOptions<AiLimitsOptions> options) : IUserTierService
{
    private readonly AiLimitsOptions _opts = options.Value;

    public UserTier Resolve(string? email)
    {
        var tierName = _opts.DefaultTier;

        if (!string.IsNullOrWhiteSpace(email))
        {
            var match = _opts.UserOverrides
                .FirstOrDefault(kv => string.Equals(kv.Key, email, StringComparison.OrdinalIgnoreCase));
            if (match.Value is not null) tierName = match.Value;
        }

        var limits = _opts.Tiers.TryGetValue(tierName, out var t)
            ? t
            : new TierLimits { DailyLimit = 30, PerMinute = 10 }; // safe fallback if tier missing from config

        return new UserTier(tierName, limits.DailyLimit, limits.PerMinute);
    }
}
