using VehicleMaintenance.DTOs.MaintenanceRecords;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IMaintenanceRecordService
    {
        Task<List<MaintenanceRecordDto>> GetAllMaintenanceRecordsAsync();
        Task<MaintenanceRecordDto?> GetMaintenanceRecordByIdAsync(int id);
        Task<List<MaintenanceRecordDto>> GetMaintenanceRecordByVehicleAsync(int vehicleId, DateTime? fromDate, DateTime? toDate, string? serviceType); // GetByVehicleAsync already present in the FuelEntryService??
        Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto dto);
        Task<MaintenanceRecordDto?> UpdateMaintenanceRecordByIdAsync(int id, UpdateMaintenanceRecordDto dto);
        Task<bool> DeleteMaintenanceRecordByIdAsync(int id);
    }
}
