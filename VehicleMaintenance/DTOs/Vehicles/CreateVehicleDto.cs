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

        public VehicleType VehicleType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public EngineType EngineType { get; set; }
        public FuelType FuelType { get; set; }

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }
    }
}