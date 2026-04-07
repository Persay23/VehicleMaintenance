using VehicleMaintenance.DTOs.MaintenanceRecordComponents;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IMaintenanceRecordComponentService
    {
        Task<List<MaintenanceRecordComponentDto>> GetAllAsync();
        Task<MaintenanceRecordComponentDto> PostAsync(CreateMaintenanceRecordComponentDto dto);
        Task<MaintenanceRecordComponentDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<MaintenanceRecordComponentDto?> PatchAsync(int id, UpdateMaintenanceRecordComponentDto dto);
    }
}
