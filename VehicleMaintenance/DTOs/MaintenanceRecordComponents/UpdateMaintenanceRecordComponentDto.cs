using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecordComponents
{
    public class UpdateMaintenanceRecordComponentDto
    {
        public string? ComponentChangeType { get; set; }
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public string? OldState { get; set; }
        public string NewState { get; set; } = null!;
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
