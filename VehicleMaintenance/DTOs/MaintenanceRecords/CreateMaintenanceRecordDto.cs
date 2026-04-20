using VehicleMaintenance.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class CreateMaintenanceRecordDto
    {
        public int VehicleId { get; set; }
        public int? PredictionId { get; set; }
        public string ServiceName { get; set; } = null!;

        [Required]
        public DateTime ServiceDate { get; set; }

        [Required]
        public string ServiceType { get; set; } = null!; // e.g., "Oil Change", "Tire Rotation", "Brake Inspection"

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        public string? Description { get; set; }
    }
}
