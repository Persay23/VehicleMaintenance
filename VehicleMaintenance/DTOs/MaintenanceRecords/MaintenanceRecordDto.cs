using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; }
        public int VehicleId { get; set; }
        public int? ComponentId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; } = null!; // e.g., "Oil Change", "Tire Rotation", "Brake Inspection"
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
