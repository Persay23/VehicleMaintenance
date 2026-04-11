using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface ILiquidEntryService
    {
        Task<LiquidEntryDto> CreateLiquidEntryAsync(CreateLiquidEntryDto dto);
        Task<List<LiquidEntryDto>> GetAllLiquidEntriesAsync();
        Task<LiquidEntryDto?> GetLiquidEntryByIdAsync(int id);
        Task<LiquidEntryDto?> UpdateLiquidEntryByIdAsync(int id, UpdateLiquidEntryDto dto);
        Task<bool> DeleteLiquidEntryByIdAsync(int id);
        Task<List<LiquidEntryDto>> GetByVehicleAsync(int vehicleId, LiquidType? liquidType, DateTime? fromDate, DateTime? toDate);
    }
}
