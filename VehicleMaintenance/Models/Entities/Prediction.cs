using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class Prediction
    {
        public int PredictionId { get; private set; }
        public int VehicleId { get; set; }
        //public string PredictionName { get; set; } = null!; // do i need name for the ai prediction and do i need this class at all or how ai will create it on its own
        public ComponentType ComponentType { get; set; }
        public DateTime PredictedServiceDate { get; set; }
        public double ConfidenceScore { get; set; } // 0.0 - 1.0
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Vehicle Vehicle { get; set; } = null!;
        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = [];
    }
}
