using VehicleMaintenance.DTOs.MaintenanceRecords;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IMaintenanceRecordService
    {
        Task<List<MaintenanceRecordDto>> GetAllAsync();
        Task<MaintenanceRecordDto> PostAsync(CreateMaintenanceRecordDto dto);
        Task<MaintenanceRecordDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<MaintenanceRecordDto?> PatchAsync(int id, UpdateMaintenanceRecordDto dto);
    }
}
