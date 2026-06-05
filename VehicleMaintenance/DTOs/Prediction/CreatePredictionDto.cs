namespace VehicleMaintenance.DTOs.Prediction
{
    // Predictions are AI-generated only. This DTO is kept minimal for internal use.
    public class CreatePredictionDto
    {
        public int VehicleId { get; set; }
        public int? VehicleComponentId { get; set; }
    }
}
