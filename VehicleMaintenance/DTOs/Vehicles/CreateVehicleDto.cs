using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Vehicles
{
    public class CreateVehicleDto
    {
        public int UserId { get; set; }

        [Required]
        public required string Brand { get; set; }

        [Required]
        public required string Model { get; set; }

        [Range(1886, 2030)]
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