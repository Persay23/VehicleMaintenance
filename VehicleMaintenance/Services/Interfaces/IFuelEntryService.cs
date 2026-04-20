using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IFuelEntryService
    {
        Task<FuelEntryDto> CreateFuelEntryAsync(CreateFuelEntryDto dto);
        Task<List<FuelEntryDto>> GetAllFuelEntriesAsync();
        Task<FuelEntryDto?> GetFuelEntryByIdAsync(int id);
        Task<FuelEntryDto?> UpdateFuelEntryByIdAsync(int id, UpdateFuelEntryDto dto);
        Task<bool> DeleteFuelEntryByIdAsync(int id);
        Task<List<FuelEntryDto>> GetByVehicleAsync(int vehicleId, string? fuelType, DateTime? fromDate, DateTime? toDate);
    }
}
