
namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class ComponentHealthDto
    {
        public int ComponentId { get; set; }
        public string? VehicleComponentName { get; set; }
        public string? VehicleComponentBrand { get; set; }
        public string ComponentType { get; set; } = null!;
        public string CurrentState { get; set; } = null!;
        public int RemainingKm { get; set; }
        public double KmLifetimePercent { get; set; }
        public double YearsLifetimePercent { get; set; }
        public string Status { get; set; } = null!; // "Good", "Monitor", "Warning", "Critical"
    }
}
