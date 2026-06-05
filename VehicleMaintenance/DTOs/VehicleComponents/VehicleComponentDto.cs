using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class VehicleComponentDto
    {
        public int VehicleComponentId { get; set; }
        public int VehicleId { get; set; }
        public string? VehicleComponentName { get; set; }
        public string? VehicleComponentBrand { get; set; }
        public string ComponentType { get; set; } = null!; // e.g., "Engine", "Brakes", "Tires", "Transmission"
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public string State { get; set; } = "Unknown"; // default to "Unknown" if not provided, can be "Good", "Fair", "Poor", etc.
        public string? Notes { get; set; }
        public int InstalledAtVehicleMileage { get; set; } // vehicle odometer at time of installation
        public int VehicleCurrentMileage { get; set; } // vehicle's current odometer (for km-used calc)
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
        public string? PartNumber { get; set; }
        public int? WarrantyKm { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public int? NextServiceRecommendedKm { get; set; }
        public DateTime? NextServiceRecommendedDate { get; set; }

        // AI prediction fields
        public DateTime? AiEstimatedNextServiceDate { get; set; }
        public int?      AiEstimatedRemainingKm     { get; set; }
        public double?   AiConfidenceScore          { get; set; }
        public string?   AiRecommendation           { get; set; }
        public string?   AiReasoning               { get; set; }
        public DateTime? AiGeneratedAt             { get; set; }
        public int?      AiHealthPercent           { get; set; }
    }
}
