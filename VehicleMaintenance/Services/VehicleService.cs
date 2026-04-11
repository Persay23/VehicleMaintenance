using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Models.Enums;
using System.Linq;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context, IMapper mapper) : IVehicleService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto)
        {
            var vehicle = _mapper.Map<Vehicle>(dto);

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return _mapper.Map<List<VehicleDto>>(vehicles);
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
            if (!string.IsNullOrWhiteSpace(dto.VehicleType)) vehicle.VehicleType = Enum.Parse<VehicleType>(dto.VehicleType, true);
            if (!string.IsNullOrWhiteSpace(dto.TransmissionType)) vehicle.TransmissionType = Enum.Parse<TransmissionType>(dto.TransmissionType, true);
            if (!string.IsNullOrWhiteSpace(dto.EngineType)) vehicle.EngineType = Enum.Parse<EngineType>(dto.EngineType, true);
            if (!string.IsNullOrWhiteSpace(dto.FuelType)) vehicle.FuelType = Enum.Parse<FuelType>(dto.FuelType, true);
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

        public async Task<VehicleCostSummaryDto?> GetCostSummaryAsync(int vehicleId, DateTime? from, DateTime? to)
        {
            var exists = await _context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId);
            if (!exists) return null;

            // MaintenanceRecords has ServiceDate and Cost - matches your entity
            var maintenanceQuery = _context.MaintenanceRecords
                .Where(mr => mr.VehicleId == vehicleId);

            // LiquidEntries has RefillDate and Cost - matches your entity
            var liquidQuery = _context.LiquidEntries
                .Where(le => le.VehicleId == vehicleId);

            if (from.HasValue)
            {
                maintenanceQuery = maintenanceQuery.Where(mr => mr.ServiceDate >= from.Value);
                liquidQuery = liquidQuery.Where(le => le.RefillDate >= from.Value);
            }
            if (to.HasValue)
            {
                maintenanceQuery = maintenanceQuery.Where(mr => mr.ServiceDate <= to.Value);
                liquidQuery = liquidQuery.Where(le => le.RefillDate <= to.Value);
            }

            var maintenanceCost = await maintenanceQuery.SumAsync(mr => mr.Cost);
            var liquidCost = await liquidQuery.SumAsync(le => le.Cost);

            return new VehicleCostSummaryDto
            {
                TotalMaintenanceCost = maintenanceCost,
                TotalLiquidCost = liquidCost,
                TotalCost = maintenanceCost + liquidCost
            };
        }

        public async Task<List<TimelineEventDto>> GetTimelineAsync(int vehicleId)
        {
            // ServiceType is an enum - ToString() converts it to readable name
            var maintenance = await _context.MaintenanceRecords
                .Where(mr => mr.VehicleId == vehicleId)
                .Select(mr => new TimelineEventDto
                {
                    Date = mr.ServiceDate,
                    Type = "Maintenance",
                    Description = mr.Description ?? mr.ServiceType.ToString(),
                    Cost = mr.Cost
                }).ToListAsync();

            // LiquidType is an enum - ToString() converts it to readable name
            var liquids = await _context.LiquidEntries
                .Where(le => le.VehicleId == vehicleId)
                .Select(le => new TimelineEventDto
                {
                    Date = le.RefillDate,
                    Type = "Liquid",
                    Description = le.LiquidType.ToString(),
                    Cost = le.Cost
                }).ToListAsync();

            return [.. maintenance
                .Concat(liquids)
                .OrderByDescending(e => e.Date)];
        }
    }
}