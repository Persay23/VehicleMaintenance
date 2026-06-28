using System.Collections.Concurrent;

namespace VehicleMaintenance.Services.RateLimiting;

// Singleton. A hard money cap on top of the per-minute burst limiter.

public class AiUsageLimiter : IAiUsageLimiter
{
    private sealed class Counter
    {
        public DateOnly Date;
        public int Count;
    }

    private readonly ConcurrentDictionary<string, Counter> _counts = new();

    public bool TryConsume(string userId, int dailyLimit, out AiQuotaStatus status)
    {
        var counter = _counts.GetOrAdd(userId, _ => new Counter());
        lock (counter)
        {
            RollOverIfNeeded(counter);

            if (dailyLimit > 0 && counter.Count >= dailyLimit)
            {
                status = Snapshot(counter.Count, dailyLimit);
                return false;
            }

            counter.Count++;
            status = Snapshot(counter.Count, dailyLimit);
            return true;
        }
    }

    public AiQuotaStatus Check(string userId, int dailyLimit)
    {
        var counter = _counts.GetOrAdd(userId, _ => new Counter());
        lock (counter)
        {
            RollOverIfNeeded(counter);
            return Snapshot(counter.Count, dailyLimit);
        }
    }

    private static void RollOverIfNeeded(Counter counter)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (counter.Date != today)
        {
            counter.Date = today;
            counter.Count = 0;
        }
    }

    private static AiQuotaStatus Snapshot(int used, int dailyLimit)
    {
        var resetsAt = DateTime.UtcNow.Date.AddDays(1); // next UTC midnight
        return dailyLimit <= 0
            ? new AiQuotaStatus(used, 0, int.MaxValue, resetsAt, Unlimited: true)
            : new AiQuotaStatus(used, dailyLimit, Math.Max(0, dailyLimit - used), resetsAt, Unlimited: false);
    }
}
