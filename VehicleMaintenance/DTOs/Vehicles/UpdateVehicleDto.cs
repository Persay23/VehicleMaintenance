using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Vehicles
{
    public class UpdateVehicleDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }

        [Range(1886, 2030)]
        public int? YearOfProduction { get; set; }

        public string? VehicleType { get; set; }
        public string? TransmissionType { get; set; }
        public string? EngineType { get; set; }
        public string? FuelType { get; set; }

        [Range(0, int.MaxValue)]
        public int? Mileage { get; set; }
    }
}
