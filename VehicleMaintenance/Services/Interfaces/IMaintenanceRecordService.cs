using VehicleMaintenance.DTOs.MaintenanceRecords;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IMaintenanceRecordService
    {
        Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto dto);
        Task<List<MaintenanceRecordDto>> GetAllMaintenanceRecordsAsync();
        Task<MaintenanceRecordDto?> GetMaintenanceRecordByIdAsync(int id);
        Task<MaintenanceRecordDto?> UpdateMaintenanceRecordByIdAsync(int id, UpdateMaintenanceRecordDto dto);
        Task<bool> DeleteMaintenanceRecordByIdAsync(int id);
    }
}
