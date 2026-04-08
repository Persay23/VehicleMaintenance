using VehicleMaintenance.DTOs.LiquidEntry;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface ILiquidEntryService
    {
        Task<LiquidEntryDto> CreateLiquidEntryAsync(CreateLiquidEntryDto dto);
        Task<List<LiquidEntryDto>> GetAllLiquidEntriesAsync();
        Task<LiquidEntryDto?> GetLiquidEntryByIdAsync(int id);
        Task<LiquidEntryDto?> UpdateLiquidEntryByIdAsync(int id, UpdateLiquidEntryDto dto);
        Task<bool> DeleteLiquidEntryByIdAsync(int id);
    }
}
