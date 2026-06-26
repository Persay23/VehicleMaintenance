using VehicleMaintenance.DTOs.Prediction;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<PredictionDto[]> GetAllPredictionsAsync();
        Task<PredictionDto?> GetPredictionByIdAsync(int id);
        Task<PredictionDto[]> GetPredictionsByVehicleAsync(int vehicleId);
        Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto);
        Task<bool> DeletePredictionByIdAsync(int id);
    }
}
