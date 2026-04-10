using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; private set; }
        public string UserId { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int YearOfProduction { get; set; }
        public VehicleType VehicleType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public EngineType EngineType { get; set; }
        public FuelType FuelType { get; set; }
        public int Mileage { get; set; }
        public User User { get; set; } = null!;
        public List<VehicleComponent> VehicleComponents { get; set; } = [];
        public List<LiquidEntry> LiquidEntries { get; set; } = [];
        public List<MaintenanceRecord> MaintenanceRecords { get; set; } = [];
        public List<Prediction> Predictions { get; set; } = [];
    }
}
