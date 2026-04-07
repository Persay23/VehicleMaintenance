using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class VehicleComponentDto
    {
        public int ComponentId { get; set; }
        public int VehicleId { get; set; }
        public ComponentType ComponentType { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public State State { get; set; }
        public string? Notes { get; set; }
        public int CurrentMileage { get; set; }
        public int ExpectedLifetimeKm { get; set; } // in km
        public int ExpectedLifetimeYears { get; set; } // in years or maybe days/months
    }
}
