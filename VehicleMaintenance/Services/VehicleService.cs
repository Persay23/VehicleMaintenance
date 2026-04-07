using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto) // interfaces
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
                UserId = vehicle.UserId,
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

        public async Task<List<VehicleDto>> GetVehiclesByUserIdAsync(int userId)
        {
            return await _context.Vehicles
                .Where(v => v.UserId == userId)
                .Select(v => new VehicleDto
                {
                    VehicleId = v.VehicleId,
                    UserId = v.UserId,
                    Brand = v.Brand,
                    Model = v.Model,
                    YearOfProduction = v.YearOfProduction,
                    VehicleType = v.VehicleType,
                    TransmissionType = v.TransmissionType,
                    EngineType = v.EngineType,
                    FuelType = v.FuelType,
                    Mileage = v.Mileage
                })
                .ToListAsync(); // repositories!!
        }
    }
}