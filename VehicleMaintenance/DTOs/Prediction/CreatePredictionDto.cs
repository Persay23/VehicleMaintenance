using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class CreatePredictionDto
    {
        public int VehicleId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string ComponentType { get; set; } = null!; // e.g., "Engine", "Brakes", "Tires"

        [Required]
        public DateTime PredictedServiceDate { get; set; }

        [Range(0, 100)]
        public int ConfidentScore { get; set; } 
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
