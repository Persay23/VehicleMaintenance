using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecordComponents
{
    public class MaintenanceRecordComponentDto
    {
        public int MaintenanceRecordComponentId { get; set; }
        public int MaintenanceRecordId { get; set; }
        public int ComponentId { get; set; }
        public ComponentChangeType ChangeType { get; set; }
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public State? OldState { get; set; }
        public State? NewState { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborMinutes { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }
        public string? TechnicianName { get; set; }
        public string? VendorOrShop { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
