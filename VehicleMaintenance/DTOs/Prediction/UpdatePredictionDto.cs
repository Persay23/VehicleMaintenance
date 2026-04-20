using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class UpdatePredictionDto
    {
        public string? Name { get; set; }
        public string? ComponentType { get; set; } // e.g., "Engine", "Brakes", "Tires", "Transmission"
        public DateTime? PredictedServiceDate { get; set; }

        [Range(0, 100)]
        public double? ConfidenceScore { get; set; }
        public string? Status { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
