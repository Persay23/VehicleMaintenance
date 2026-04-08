using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class UpdateMaintenanceRecordDto
    {
        public DateTime? ServiceDate { get; set; }
        public ServiceType? ServiceType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        public string? Description { get; set; }
    }
}
