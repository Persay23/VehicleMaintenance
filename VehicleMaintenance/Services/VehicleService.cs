using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context, IMapper mapper) : IVehicleService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto) // interfaces
        {
            var vehicle = _mapper.Map<Vehicle>(dto);

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return _mapper.Map<List<VehicleDto>>(vehicles); // repositories!!
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
            return vehicle is null ? null : _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<VehicleDto?> UpdateVehicleByIdAsync(int id, UpdateVehicleDto dto)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
            if (vehicle is null)
            {
                return null;
            }

            if (dto.Brand is not null) vehicle.Brand = dto.Brand;
            if (dto.Model is not null) vehicle.Model = dto.Model;
            if (dto.YearOfProduction.HasValue) vehicle.YearOfProduction = dto.YearOfProduction.Value;
            if (dto.VehicleType.HasValue) vehicle.VehicleType = dto.VehicleType.Value;
            if (dto.TransmissionType.HasValue) vehicle.TransmissionType = dto.TransmissionType.Value;
            if (dto.EngineType.HasValue) vehicle.EngineType = dto.EngineType.Value;
            if (dto.FuelType.HasValue) vehicle.FuelType = dto.FuelType.Value;
            if (dto.Mileage.HasValue) vehicle.Mileage = dto.Mileage.Value;

            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<bool> DeleteVehicleByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
            if (vehicle is null)
            {
                return false;
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}