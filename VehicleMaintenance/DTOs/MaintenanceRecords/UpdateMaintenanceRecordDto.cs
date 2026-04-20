using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class UpdateMaintenanceRecordDto
    {
        public DateTime? ServiceDate { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceType { get; set; } // e.g., "Oil Change", "Tire Rotation", "Brake Inspection"

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        public string? Description { get; set; }
    }
}
