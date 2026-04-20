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
        public int CurrentMileage { get; set; }
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
    }
}
