using VehicleMaintenance.DTOs.MaintenanceRecordComponents;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IMaintenanceRecordComponentService
    {
        Task<MaintenanceRecordComponentDto[]> GetAllMaintenanceRecordComponentsAsync();
        Task<MaintenanceRecordComponentDto?> GetMaintenanceRecordComponentByIdAsync(int id);
        Task<MaintenanceRecordComponentDto> CreateMaintenanceRecordComponentAsync(CreateMaintenanceRecordComponentDto dto);
        Task<MaintenanceRecordComponentDto?> UpdateMaintenanceRecordComponentByIdAsync(int id, UpdateMaintenanceRecordComponentDto dto);
        Task<bool> DeleteMaintenanceRecordComponentByIdAsync(int id);
    }
}
