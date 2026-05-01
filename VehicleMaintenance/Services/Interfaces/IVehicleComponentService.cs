using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleComponentService
    {
        Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto);
        Task<List<VehicleComponentDto>> GetAllVehicleComponentsAsync();
        Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id);
        Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto);
        Task<bool> DeleteVehicleComponentByIdAsync(int id);
        Task<List<ComponentHealthDto>> GetComponentHealthAsync(int vehicleId);
        Task<List<VehicleComponentDto>> GetByVehicleAsync(int vehicleId);
        Task<List<ComponentHistoryDto>> GetComponentHistoryAsync(int componentId);
    }
}
