using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecordComponents
{
    public class CreateMaintenanceRecordComponentDto
    {
        [Required]
        public int MaintenanceRecordId { get; set; }

        [Required]
        public int ComponentId { get; set; }

        [Required]
        public string ComponentChangeType { get; set; } = null!;

        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }// maybe this should be a list of changed parts instead of a string
        public string? OldState { get; set; } // e.g., "Worn", "Damaged", "Functional"
        public string NewState { get; set; } = null!; // e.g., "New", "Repaired", "Functional"
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        [Range(0, int.MaxValue)]
        public int? LaborDays { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? LaborCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PartsCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OtherCost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TotalCost { get; set; }

        public string? TechnicianName { get; set; }
        public string? Vendor { get; set; }
        public string? Notes { get; set; }
    }
}
