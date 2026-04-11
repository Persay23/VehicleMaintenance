using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.LiquidEntry
{
    public class LiquidEntryDto
    {
        public int LiquidEntryId { get; set; }
        public int VehicleId { get; set; }                 
        public string LiquidType { get; set; } = null!; // e.g., "Engine Oil", "Coolant", "Brake Fluid"
        public DateTime RefillDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public int Mileage { get; set; }
        public string? Notes { get; set; }
    }
}
