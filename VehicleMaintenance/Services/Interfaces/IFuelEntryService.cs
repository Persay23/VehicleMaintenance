using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IFuelEntryService
    {
        Task<FuelEntryDto[]> GetAllFuelEntriesAsync();
        Task<FuelEntryDto?> GetFuelEntryByIdAsync(int id);
        Task<FuelEntryDto[]> GetFuelEntryByVehicleAsync(int vehicleId, string? fuelType, DateTime? fromDate, DateTime? toDate);
        Task<FuelEntryDto> CreateFuelEntryAsync(CreateFuelEntryDto dto);
        Task<FuelEntryDto?> UpdateFuelEntryByIdAsync(int id, UpdateFuelEntryDto dto);
        Task<bool> DeleteFuelEntryByIdAsync(int id);
    }
}
