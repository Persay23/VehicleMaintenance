using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class FuelEntry
    {
        public int FuelEntryId { get; private set; }
        public int VehicleId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime RefillDate { get; set; }
        public decimal Amount { get; set; } // liters
        public decimal Cost { get; set; }
        public int Mileage { get; set; }
        public string? Notes { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
