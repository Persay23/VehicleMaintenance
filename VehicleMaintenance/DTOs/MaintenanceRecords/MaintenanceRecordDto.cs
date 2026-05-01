using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; }
        public int VehicleId { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime ServiceDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }
        public string ServiceType { get; set; } = null!;
        public int? Mileage { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? TechnicianName { get; set; }
        public string? VendorOrShop { get; set; }
        public string? Notes { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImageUrl { get; set; }
        public VehicleDto? Vehicle { get; set; }
        public List<MaintenanceRecordComponentDto> MaintenanceRecordComponents { get; set; } = [];
    }
}
