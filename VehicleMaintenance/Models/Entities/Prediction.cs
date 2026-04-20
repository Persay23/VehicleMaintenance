using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class Prediction
    {
        public int PredictionId { get; private set; }
        public int VehicleId { get; set; }
        //public int? VehicleComponentId { get; set; } // 

        public int VehicleComponentId { get; internal set; }
        public string Name { get; set; } = null!;
        public ComponentType ComponentType { get; set; }  // Electric car → EngineType = Electric, FuelType = Electric → redundant
        public DateTime PredictedServiceDate { get; set; }
        public double ConfidenceScore { get; set; } // 0.0 - 1.0
        public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;
        public PredictionStatus Status { get; set; } = PredictionStatus.Active;
        public DateTime? CompletedAt { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public List<Vehicle> Vehicles { get; set; } = [];
    }
}
