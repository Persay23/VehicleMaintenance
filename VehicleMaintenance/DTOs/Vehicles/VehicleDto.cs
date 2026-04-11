using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Vehicles
{
    public class VehicleDto
    {
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int YearOfProduction { get; set; }
        public string VehicleType { get; set; } = null!; // e.g., "Sedan", "SUV", "Truck", "Motorcycle"
        public string TransmissionType { get; set; } = null!; // e.g., "Automatic", "Manual"
        public string EngineType { get; set; } = null!; // e.g., "hybrid", "combustion", "Electric"
        public string FuelType { get; set; } = null!; // e.g., "Gasoline", "Diesel", "Electric"
        public int Mileage { get; set; }
    }
}