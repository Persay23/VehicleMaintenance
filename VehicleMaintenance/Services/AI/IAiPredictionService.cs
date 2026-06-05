using VehicleMaintenance.DTOs.AI;

namespace VehicleMaintenance.Services.AI;

public interface IAiPredictionService
{
    void TriggerBackgroundUpdate(int componentId, int vehicleId);
    Task GenerateComponentPredictionAsync(int componentId, bool forceRefresh = false);
    Task GenerateVehicleSuggestionsAsync(int vehicleId, bool forceRefresh = false);
    Task<DiagnoseResponseDto?> DiagnoseAsync(int vehicleId, string symptom);
}
