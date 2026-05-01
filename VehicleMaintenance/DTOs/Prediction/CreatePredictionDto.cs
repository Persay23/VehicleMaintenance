using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class CreatePredictionDto
    {
        public int VehicleId { get; set; }
        [Required]
        public int VehicleComponentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string ComponentType { get; set; } = null!; // e.g., "Engine", "Brakes", "Tires"

        [Required]
        public DateTime PredictedServiceDate { get; set; }

        [Range(0, 100)]
        public double ConfidenceScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
