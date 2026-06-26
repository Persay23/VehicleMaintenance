using Microsoft.Extensions.Options;

namespace VehicleMaintenance.Services.RateLimiting;

/// <summary>A user's resolved AI tier and its limits (0 = unlimited).</summary>
public record UserTier(string Name, int DailyLimit, int PerMinute);

public interface IUserTierService
{
    /// <summary>Resolves the tier for a user by email (config override map → default tier).</summary>
    UserTier Resolve(string? email);
}

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
