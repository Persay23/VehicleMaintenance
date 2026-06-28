using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Services.GenralModelService.Interfaces
{
    public interface IVehicleComponentService
    {
        Task<VehicleComponentDto[]> GetAllVehicleComponentsAsync();
        Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id);
        Task<VehicleComponentDto[]> GetVehicleComponentByVehicleAsync(int vehicleId);
        Task<ComponentHealthDto[]> GetComponentHealthAsync(int vehicleId);
        Task<ComponentHistoryDto[]> GetComponentHistoryAsync(int componentId);
        Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto);
        Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto);
        Task<bool> DeleteVehicleComponentByIdAsync(int id);
    }
}
