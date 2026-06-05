using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleComponentService
    {
        Task<List<VehicleComponentDto>> GetAllVehicleComponentsAsync();
        Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id);
        Task<List<VehicleComponentDto>> GetVehicleComponentByVehicleAsync(int vehicleId);
        Task<List<ComponentHealthDto>> GetComponentHealthAsync(int vehicleId);
        Task<List<ComponentHistoryDto>> GetComponentHistoryAsync(int componentId);
        Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto);
        Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto);
        Task<bool> DeleteVehicleComponentByIdAsync(int id);
    }
}
