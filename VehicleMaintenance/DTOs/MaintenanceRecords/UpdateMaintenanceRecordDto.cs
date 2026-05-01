using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class UpdateMaintenanceRecordDto
    {
        public string? ServiceName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }
        public string? ServiceType { get; set; }
        public int? Mileage { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        public string? Description { get; set; }
        public string? TechnicianName { get; set; }
        public string? VendorOrShop { get; set; }
        public string? Notes { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImageUrl { get; set; }
    }
}
