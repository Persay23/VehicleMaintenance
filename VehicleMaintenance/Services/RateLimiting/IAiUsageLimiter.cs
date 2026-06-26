namespace VehicleMaintenance.Services.RateLimiting;

/// <summary>Snapshot of a user's AI usage for today.</summary>
public record AiQuotaStatus(int Used, int Limit, int Remaining, DateTime ResetsAtUtc, bool Unlimited);

public interface IAiUsageLimiter
{
    /// <summary>Reads current usage without consuming (for the quota endpoint).</summary>
    AiQuotaStatus Check(string userId, int dailyLimit);

    /// <summary>Consumes one AI call for today. Returns false (and an unchanged snapshot) when the daily cap is hit.</summary>
    bool TryConsume(string userId, int dailyLimit, out AiQuotaStatus status);
}
