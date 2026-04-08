using VehicleMaintenance.DTOs.Vehicles;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto);
        Task<List<VehicleDto>> GetAllVehiclesAsync();
        Task<VehicleDto?> GetVehicleByIdAsync(int id);
        Task<VehicleDto?> UpdateVehicleByIdAsync(int id, UpdateVehicleDto dto);
        Task<bool> DeleteVehicleByIdAsync(int id);
    }
}
