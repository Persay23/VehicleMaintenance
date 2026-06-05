using VehicleMaintenance.DTOs.Prediction;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<List<PredictionDto>> GetAllPredictionsAsync();
        Task<PredictionDto?> GetPredictionByIdAsync(int id);
        Task<List<PredictionDto>> GetPredictionsByVehicleAsync(int vehicleId);
        Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto);
        Task<bool> DeletePredictionByIdAsync(int id);
    }
}
