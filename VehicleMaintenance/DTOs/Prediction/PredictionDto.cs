using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class PredictionDto
    {
        public int PredictionId { get; set; }
        public int VehicleId { get; set; }
        public int? VehicleComponentId { get; set; }

        // From VehicleComponent navigation (null if vehicle-level suggestion)
        public string? ComponentName { get; set; }
        public string? ComponentType { get; set; }

        // AI-generated content
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Urgency { get; set; } = null!;
        public decimal? ConfidenceScore { get; set; }
        public DateTime? SuggestedByDate { get; set; }
        public int? EstimatedRemainingKm { get; set; }

        // User action
        public string Status { get; set; } = PredictionStatus.Active.ToString();
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? IgnoredAt { get; set; }
    }
}
