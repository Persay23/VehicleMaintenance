using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class Prediction
    {
        public int PredictionId { get; set; }
        public int VehicleId { get; set; }
        public int? VehicleComponentId { get; set; }

        // AI-generated content
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Urgency { get; set; } = null!;    // Immediate | Soon | Scheduled | Suggested
        public decimal? ConfidenceScore { get; set; }
        public DateTime? SuggestedByDate { get; set; }
        public int? EstimatedRemainingKm { get; set; }

        // User action tracking
        public PredictionStatus Status { get; set; } = PredictionStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public DateTime? IgnoredAt { get; set; }

        // Navigation
        public Vehicle Vehicle { get; set; } = null!;
        public VehicleComponent? VehicleComponent { get; set; }
    }
}
