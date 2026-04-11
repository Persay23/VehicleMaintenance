using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.LiquidEntry
{
    public class UpdateLiquidEntryDto
    {
        public string? LiquidType { get; set; }
        public DateTime? RefillDate { get; set; }
        public decimal? Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        [Range(0, int.MaxValue)]
        public int? Mileage { get; set; }

        public string? Notes { get; set; }
    }
}
