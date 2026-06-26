using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services.Auth;

public interface IJwtTokenService
{
    /// <summary>Builds a signed JWT for the user and returns it with its expiry timestamp.</summary>
    (string Token, DateTime ExpiresAt) CreateToken(User user, IList<string> roles);
}
