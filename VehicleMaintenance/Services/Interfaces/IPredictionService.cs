using VehicleMaintenance.DTOs.Prediction;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<List<PredictionDto>> GetAllAsync();
        Task<PredictionDto> PostAsync(CreatePredictionDto dto);
        Task<PredictionDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<PredictionDto?> PatchAsync(int id, UpdatePredictionDto dto);
    }
}
