using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class LiquidEntry
    {
        public int LiquidEntryId { get; private set; }
        public int VehicleId { get; set; }
        public LiquidType LiquidType { get; set; } // should serve as a name?
        public DateTime RefillDate { get; set; }
        public decimal Amount { get; set; } // liters
        public decimal Cost { get; set; }
        public int Mileage { get; set; }
        public string? Notes { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
