namespace VehicleMaintenance.Services.RateLimiting;

public record UserTier(string Name, int DailyLimit, int PerMinute);

public interface IUserTierService
{
    UserTier Resolve(string? email);
}
