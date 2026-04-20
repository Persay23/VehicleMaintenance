
namespace VehicleMaintenance.DTOs.FuelEntry
{
    public class FuelEntryDto
    {
        public int FuelEntryId { get; set; }
        public int VehicleId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? FuelType { get; set; } // e.g., Gasoline, Diesel, Petrol
        public DateTime RefillDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public int Mileage { get; set; }
        public string? Notes { get; set; }
    }
}
