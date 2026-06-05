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

        // ComponentChangeType // maybe add

        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; } // shoudl updates automatically when component is linked to a record
        public State State { get; set; } // good, normal, ect
        public string? Notes { get; set; }
        public int InstalledAtVehicleMileage { get; set; } // vehicle odometer reading at time of installation
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
        public string? PartNumber { get; set; }
        public int? WarrantyKm { get; set; }
        public int? NextServiceRecommendedKm { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public DateTime? NextServiceRecommendedDate { get; set; }

        // AI prediction fields
        public DateTime? AiEstimatedNextServiceDate { get; set; }
        public int?      AiEstimatedRemainingKm     { get; set; }
        public double?   AiConfidenceScore          { get; set; }
        public string?   AiRecommendation           { get; set; }
        public string?   AiReasoning               { get; set; }
        public DateTime? AiGeneratedAt             { get; set; }
        public int?      AiHealthPercent           { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
        public List<Prediction> Predictions { get; set; } = [];
        public List<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; } = [];

    }
}
