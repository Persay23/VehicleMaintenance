using VehicleMaintenance.DTOs.Prediction;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<PredictionDto> CreatePredictionAsync(CreatePredictionDto dto);
        Task<List<PredictionDto>> GetAllPredictionsAsync();
        Task<PredictionDto?> GetPredictionByIdAsync(int id);
        Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto);
        Task<bool> DeletePredictionByIdAsync(int id);
    }
}
