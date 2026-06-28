using VehicleMaintenance.DTOs.Vehicles;

namespace VehicleMaintenance.Services.GenralModelService.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto[]> GetAllVehiclesAsync(string userId);
        Task<VehicleDto?> GetVehicleByIdAsync(int id);
        Task<MonthlyCostDto[]> GetCostSummaryAsync(int vehicleId, DateTime? from, DateTime? to);
        Task<TimelineEventDto[]> GetTimelineAsync(int vehicleId);
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, string userId);
        Task<VehicleDto?> UpdateVehicleByIdAsync(int id, UpdateVehicleDto dto);
        Task<bool> DeleteVehicleByIdAsync(int id);
    }
}
