using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.Vehicles
{    
    public class CreateVehicleDto
    {

        [Required]
        public string Brand { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Range(1886, 2100)]
        public int YearOfProduction { get; set; }

        // some of those field are redundant, define which to delete
        public string VehicleType { get; set; } = null!; // e.g., "Sedan", "SUV", "Truck", "Motorcycle"
        public string TransmissionType { get; set; } = null!; // e.g., "Automatic", "Manual"
        public string EngineType { get; set; } = null!; // e.g., "hybrid", "combustion", "Electric"
        public string FuelType { get; set; } = null!; // e.g., "Gasoline", "Diesel", "Electric"

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }
    }
}