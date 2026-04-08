using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class UpdateVehicleComponentDto
    {
        public ComponentType? ComponentType { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public State? State { get; set; }
        public string? Notes { get; set; }

        [Range(0, int.MaxValue)]
        public int? CurrentMileage { get; set; }

        [Range(0, int.MaxValue)]
        public int? ExpectedLifetimeKm { get; set; }

        [Range(0, 50)]
        public int? ExpectedLifetimeYears { get; set; }
    }
}
