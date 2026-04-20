using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class PredictionDto
    {
        public int PredictionId { get; set; }
        public int VehicleId { get; set; }
        public string Name { get; set; } = null!;
        public string ComponentType { get; set; } = null!; // e.g., "Engine", "Brakes", "Tires"
        public DateTime PredictedServiceDate { get; set; }
        public double ConfidenceScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = PredictionStatus.Active.ToString();
        public DateTime? CompletedAt { get; set; }
    }
}
