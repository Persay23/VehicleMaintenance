namespace VehicleMaintenance.Services.Security;

/// <summary>
/// Central per-user ownership checks. Every entity roots at <c>Vehicle.UserId</c> (directly, or via
/// a parent vehicle), so each method answers "does this user own the resource?" with one cheap query.
/// Controllers call these before reading/mutating a resource and return 404 when the check fails,
/// which both blocks cross-user access (IDOR) and avoids leaking that the id exists.
/// </summary>
public interface IVehicleOwnershipService
{
    Task<bool> OwnsVehicleAsync(string userId, int vehicleId);
    Task<bool> OwnsComponentAsync(string userId, int componentId);
    Task<bool> OwnsFuelEntryAsync(string userId, int fuelEntryId);
    Task<bool> OwnsMaintenanceRecordAsync(string userId, int recordId);
    Task<bool> OwnsRecordComponentAsync(string userId, int recordComponentId);
    Task<bool> OwnsExpenseAsync(string userId, int expenseId);
    Task<bool> OwnsPredictionAsync(string userId, int predictionId);
}
