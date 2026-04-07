using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleComponentService
    {
        Task<List<VehicleComponentDto>> GetAllAsync();
        Task<VehicleComponentDto> PostAsync(CreateVehicleComponentDto dto);
        Task<VehicleComponentDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<VehicleComponentDto?> PatchAsync(int id, UpdateVehicleComponentDto dto);
    }
}
