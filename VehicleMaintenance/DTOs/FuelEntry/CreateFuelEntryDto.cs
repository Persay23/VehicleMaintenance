using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.FuelEntry
{
    public class CreateFuelEntryDto
    {
        public int VehicleId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }

        [Required]
        public string FuelType { get; set; } = null!; // e.g., Gasoline, Diesel, Petrol

        public DateTime RefillDate { get; set; }

        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }
        public string? Notes { get; set; }
    }
}
