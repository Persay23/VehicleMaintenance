using VehicleMaintenance.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.MaintenanceRecords
{
    public class CreateMaintenanceRecordDto
    {
        public int VehicleId { get; set; }                
        public int? ComponentId { get; set; }

        [Required]
        public DateTime ServiceDate { get; set; }

        [Required]
        public ServiceType ServiceType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        public string? Description { get; set; }
    }
}
