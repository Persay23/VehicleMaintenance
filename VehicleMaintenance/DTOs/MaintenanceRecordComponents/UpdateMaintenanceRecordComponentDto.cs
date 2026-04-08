using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecordComponents
{
    public class UpdateMaintenanceRecordComponentDto
    {
        public ComponentChangeType? ChangeType { get; set; }
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public State? OldState { get; set; }
        public State? NewState { get; set; }
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
