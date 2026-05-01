using VehicleMaintenance.DTOs.Vehicles;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, string userId);
        Task<List<VehicleDto>> GetAllVehiclesAsync(string userId);
        Task<VehicleDto?> GetVehicleByIdAsync(int id);
        Task<VehicleDto?> UpdateVehicleByIdAsync(int id, UpdateVehicleDto dto);
        Task<bool> DeleteVehicleByIdAsync(int id);
        Task<List<MonthlyCostDto>> GetCostSummaryAsync(int vehicleId, DateTime? from, DateTime? to);
        Task<List<TimelineEventDto>> GetTimelineAsync(int vehicleId);
    }
}
