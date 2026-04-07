using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class CreateVehicleComponentDto
    {
        public int VehicleId { get; set; }

        [Required]
        public ComponentType ComponentType { get; set; }

        [Required]
        public DateTime InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public State State { get; set; } = State.Unknown;
        public string? Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int CurrentMileage { get; set; }

        [Range(0, int.MaxValue)]
        public int ExpectedLifetimeKm { get; set; }

        [Range(0, 50)]
        public int ExpectedLifetimeYears { get; set; }
    }
}