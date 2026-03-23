using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;


        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                UserId = dto.UserId,
                Brand = dto.Brand,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                VehicleType = dto.VehicleType,
                TransmissionType = dto.TransmissionType,
                EngineType = dto.EngineType,
                FuelType = dto.FuelType,
                Mileage = dto.Mileage
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                UserId = dto.UserId,
                Brand = dto.Brand,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                VehicleType = dto.VehicleType,
                TransmissionType = dto.TransmissionType,
                EngineType = dto.EngineType,
                FuelType = dto.FuelType,
                Mileage = dto.Mileage
            };
        }
    }
}
