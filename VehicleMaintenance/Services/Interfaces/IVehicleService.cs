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
        Task<VehicleCostSummaryDto?> GetCostSummaryAsync(int vehicleId, DateTime? from, DateTime? to);
        Task<List<TimelineEventDto>> GetTimelineAsync(int vehicleId);
    }
}
