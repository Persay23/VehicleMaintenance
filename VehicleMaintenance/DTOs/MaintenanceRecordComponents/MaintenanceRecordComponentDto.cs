using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecordComponents
{
    public class MaintenanceRecordComponentDto
    {
        public int MaintenanceRecordComponentId { get; set; }
        public int MaintenanceRecordId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentChangeType { get; set; } = null!; // e.g., "Inspection", "Repair", "Replacement"
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public string? OldState { get; set; } // e.g., "Worn", "Damaged", "Functional"
        public string? NewState { get; set; } // e.g., "New", "Repaired", "Functional"
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }
        public string? TechnicianName { get; set; }
        public string? Vendor { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
