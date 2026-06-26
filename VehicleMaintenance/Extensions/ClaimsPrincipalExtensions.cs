using System.Security.Claims;

namespace VehicleMaintenance.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// The current user's id. ASP.NET Identity stores it as the NameIdentifier claim, which the
    /// JWT also carries — so this works the same under cookie or bearer auth. Null if unauthenticated.
    /// </summary>
    public static string? GetUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);
}
