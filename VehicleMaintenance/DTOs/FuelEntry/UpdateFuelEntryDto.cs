using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.FuelEntry
{
    public class UpdateFuelEntryDto
    {
        public string? FuelType { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public DateTime? RefillDate { get; set; }
        public decimal? Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        [Range(0, int.MaxValue)]
        public int? Mileage { get; set; }

        public string? Notes { get; set; }
    }
}
