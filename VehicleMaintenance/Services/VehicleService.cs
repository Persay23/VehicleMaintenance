using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context, IMapper mapper) : IVehicleService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<VehicleDto>> GetAllVehiclesAsync(string userId)
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.UserId == userId)
                .ToListAsync();
            return _mapper.Map<List<VehicleDto>>(vehicles);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
            if (vehicle is null) return null;

            if (vehicle.AverageKmPerYear == null)
                await TryComputeAvgKmAsync(vehicle);

            return _mapper.Map<VehicleDto>(vehicle);
        }

        private async Task TryComputeAvgKmAsync(Vehicle vehicle)
        {
            DateTime? oldestDate     = null;
            int?      oldestMileage  = null;

            var oldestRecord = await _context.MaintenanceRecords
                .Where(r => r.VehicleId == vehicle.VehicleId && r.Mileage.HasValue)
                .OrderBy(r => r.ServiceDate)
                .FirstOrDefaultAsync();

            if (oldestRecord != null)
            {
                oldestDate    = oldestRecord.ServiceDate;
                oldestMileage = oldestRecord.Mileage;
            }

            var oldestFuel = await _context.FuelEntries
                .Where(f => f.VehicleId == vehicle.VehicleId)
                .OrderBy(f => f.RefillDate)
                .FirstOrDefaultAsync();

            if (oldestFuel != null && (oldestDate == null || oldestFuel.RefillDate < oldestDate))
            {
                oldestDate    = oldestFuel.RefillDate;
                oldestMileage = oldestFuel.Mileage;
            }

            if (oldestDate == null || oldestMileage == null) return;

            var daysDiff = (DateTime.UtcNow.Date - oldestDate.Value.Date).Days;
            var kmDiff   = vehicle.Mileage - oldestMileage.Value;

            if (daysDiff >= 30 && kmDiff > 0)
            {
                vehicle.AverageKmPerYear = (int)Math.Round(kmDiff / (daysDiff / 365.25));
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MonthlyCostDto>> GetCostSummaryAsync(int vehicleId, DateTime? from, DateTime? to)
        {
            var fromDate = from ?? DateTime.UtcNow.AddMonths(-6);
            var toDate = to ?? DateTime.UtcNow;

            var records = await _context.MaintenanceRecords
                .Where(r => r.VehicleId == vehicleId
                         && r.ServiceDate >= fromDate
                         && r.ServiceDate <= toDate)
                .GroupBy(r => new { r.ServiceDate.Year, r.ServiceDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, MaintenanceCost = g.Sum(r => r.Cost) })
                .ToListAsync();

            var fuel = await _context.FuelEntries
                .Where(f => f.VehicleId == vehicleId
                         && f.RefillDate >= fromDate
                         && f.RefillDate <= toDate)
                .GroupBy(f => new { f.RefillDate.Year, f.RefillDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, FuelCost = g.Sum(f => f.Cost) })
                .ToListAsync();

            // O(1) lookup
            var recordMap = records.ToDictionary(r => (r.Year, r.Month), r => r.MaintenanceCost);
            var fuelMap = fuel.ToDictionary(f => (f.Year, f.Month), f => f.FuelCost);

            var months = recordMap.Keys
                .Union(fuelMap.Keys)
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month);

            return [.. months.Select(m => new MonthlyCostDto
            {
                Month           = new DateTime(m.Year, m.Month, 1),
                MaintenanceCost = recordMap.GetValueOrDefault(m),
                FuelCost        = fuelMap.GetValueOrDefault(m),
            })];
        }

        public async Task<List<TimelineEventDto>> GetTimelineAsync(int vehicleId) // Add pagination
        {
            var maintenance = await _context.MaintenanceRecords
                .Where(mr => mr.VehicleId == vehicleId)
                .Select(mr => new TimelineEventDto
                {
                    Date = mr.ServiceDate,
                    Type = "Maintenance",
                    Description = mr.Description ?? mr.ServiceType.ToString(),
                    Cost = mr.Cost,
                    RelatedId = mr.MaintenanceRecordId
                }).ToListAsync();

            var fuels = await _context.FuelEntries
                .Where(le => le.VehicleId == vehicleId)
                .Select(le => new TimelineEventDto
                {
                    Date = le.RefillDate,
                    Type = "Fuel",
                    Description = le.FuelType.ToString(),
                    Cost = le.Cost,
                    RelatedId = le.FuelEntryId
                }).ToListAsync();

            return [.. maintenance
                .Concat(fuels)
                .OrderByDescending(e => e.Date)];
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto, string userId)
        {
            var vehicle = _mapper.Map<Vehicle>(dto);
            vehicle.UserId = userId;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(vehicle);
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
    }
}