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
        public int ConfidentScore { get; set; } 
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
