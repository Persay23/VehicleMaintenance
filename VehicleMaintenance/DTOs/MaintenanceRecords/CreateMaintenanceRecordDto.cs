using VehicleMaintenance.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class CreateMaintenanceRecordDto
    {
        public int VehicleId { get; set; }
        public int? PredictionId { get; set; }
        [Required]
        public string ServiceName { get; set; } = null!;

        [Required]
        public DateTime ServiceDate { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }

        [Required]
        public string ServiceType { get; set; } = null!;

        public int? Mileage { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        public string? Description { get; set; }
        public string? TechnicianName { get; set; }
        public string? VendorOrShop { get; set; }
        public string? Notes { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceImageUrl { get; set; }
    }
}
