using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; private set; }
        public int VehicleId { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime ServiceDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }
        public ServiceType ServiceType { get; set; }
        public int? Mileage { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? TechnicianName { get; set; }
        public string? VendorOrShop { get; set; }
        public string? Notes { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImageUrl { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public List<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; } = [];

    }
}
