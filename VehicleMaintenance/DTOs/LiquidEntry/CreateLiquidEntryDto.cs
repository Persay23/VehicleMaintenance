using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.LiquidEntry
{
    public class CreateLiquidEntryDto
    {
        public int VehicleId { get; set; }

        [Required]
        public LiquidType LiquidType { get; set; }

        [Required]
        public DateTime RefillDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }
        public string? Notes { get; set; }
    }
}
