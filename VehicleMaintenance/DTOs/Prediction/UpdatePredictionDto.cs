namespace VehicleMaintenance.DTOs.Prediction
{
    public class UpdatePredictionDto
    {
        public string? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? IgnoredAt { get; set; }
    }
}
