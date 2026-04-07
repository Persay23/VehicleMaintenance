using VehicleMaintenance.DTOs.LiquidEntry;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface ILiquidEntryService
    {
        Task<List<LiquidEntryDto>> GetAllAsync();
        Task<LiquidEntryDto> PostAsync(CreateLiquidEntryDto dto);
        Task<LiquidEntryDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<LiquidEntryDto?> PatchAsync(int id, UpdateLiquidEntryDto dto);
    }
}
