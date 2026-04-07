using VehicleMaintenance.DTOs.Vehicles;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync();
        Task<VehicleDto> PostAsync(CreateVehicleDto dto);
        Task<VehicleDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<VehicleDto?> PatchAsync(int id, UpdateVehicleDto dto);
    }
}
