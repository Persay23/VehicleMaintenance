using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class VehicleComponent
    {
        [Key]
        public int VehicleComponentId { get; private set; } 
        public int VehicleId { get; set; }
        public string? VehicleComponentName { get; set; } // name should be obligatory
        public string? VehicleComponentBrand { get; set; }   // "Brembo", "Varta", "Gates"
        public ComponentType ComponentType { get; set; } // engine, transmission
        // ComponentChangeType
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; } // shoudl updates automatically when component is linked to a record
        public State State { get; set; } // good, normal, ect
        public string? Notes { get; set; }
        public int CurrentMileage { get; set; }
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
        public string? PartNumber { get; set; }
        public int? WarrantyKm { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public int? NextServiceRecommendedKm { get; set; }
        public DateTime? NextServiceRecommendedDate { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public List<Prediction> Predictions { get; set; } = [];
        public List<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; } = [];

    }
}
