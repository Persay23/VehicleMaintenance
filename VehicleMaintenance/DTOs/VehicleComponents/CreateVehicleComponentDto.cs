using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class CreateVehicleComponentDto
    {
        public int VehicleId { get; set; }

        [Required]
        public string ComponentType { get; set; } = null!; // e.g., "Engine", "Brakes", "Tires", "Transmission"

        [Required]
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public string State { get; set; } = "Unknown"; // default to "Unknown" if not provided, can be "Good", "Fair", "Poor", etc.
        public string? Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int CurrentMileage { get; set; }

        [Range(0, int.MaxValue)]
        public int ExpectedLifetimeKm { get; set; }

        [Range(0, 50)]
        public int ExpectedLifetimeYears { get; set; }
    }
}