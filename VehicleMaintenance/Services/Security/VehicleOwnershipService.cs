using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;

namespace VehicleMaintenance.Services.Security;

/// <inheritdoc cref="IVehicleOwnershipService"/>
public class VehicleOwnershipService(AppDbContext db) : IVehicleOwnershipService
{
    private readonly AppDbContext _db = db;

    public Task<bool> OwnsVehicleAsync(string userId, int vehicleId) =>
        _db.Vehicles.AsNoTracking()
           .AnyAsync(v => v.VehicleId == vehicleId && v.UserId == userId);

    public Task<bool> OwnsComponentAsync(string userId, int componentId) =>
        _db.VehicleComponents.AsNoTracking()
           .AnyAsync(c => c.VehicleComponentId == componentId && c.Vehicle.UserId == userId);

    public Task<bool> OwnsFuelEntryAsync(string userId, int fuelEntryId) =>
        _db.FuelEntries.AsNoTracking()
           .AnyAsync(f => f.FuelEntryId == fuelEntryId && f.Vehicle.UserId == userId);

    public Task<bool> OwnsMaintenanceRecordAsync(string userId, int recordId) =>
        _db.MaintenanceRecords.AsNoTracking()
           .AnyAsync(r => r.MaintenanceRecordId == recordId && r.Vehicle.UserId == userId);

    public Task<bool> OwnsRecordComponentAsync(string userId, int recordComponentId) =>
        _db.MaintenanceRecordComponents.AsNoTracking()
           .AnyAsync(mrc => mrc.MaintenanceRecordComponentId == recordComponentId
                            && mrc.MaintenanceRecord.Vehicle.UserId == userId);

    public Task<bool> OwnsExpenseAsync(string userId, int expenseId) =>
        _db.GeneralExpenses.AsNoTracking()
           .AnyAsync(e => e.GeneralExpenseId == expenseId && e.Vehicle.UserId == userId);

    public Task<bool> OwnsPredictionAsync(string userId, int predictionId) =>
        _db.Predictions.AsNoTracking()
           .AnyAsync(p => p.PredictionId == predictionId && p.Vehicle.UserId == userId);
}
