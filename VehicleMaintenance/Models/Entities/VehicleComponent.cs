using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class VehicleComponent
    {
        [Key]
        public int VehicleComponentId { get; private set; } 
        public int VehicleId { get; set; }
        public string? VehicleComponentName { get; set; }
        public string? VehicleComponentBrand { get; set; }   // "Brembo", "Varta", "Gates"
        public ComponentType ComponentType { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public State State { get; set; }
        public string? Notes { get; set; }
        public int CurrentMileage { get; set; }
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
        public Vehicle Vehicle { get; set; } = null!;
        public List<Prediction> Predictions { get; set; } = [];
        public List<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; } = [];

    }
}
