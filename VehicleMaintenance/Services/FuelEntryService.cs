using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class FuelEntryService(AppDbContext context, IMapper mapper) : IFuelEntryService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<FuelEntryDto> CreateFuelEntryAsync(CreateFuelEntryDto dto)
        {
            var FuelEntry = _mapper.Map<FuelEntry>(dto);

            _context.FuelEntries.Add(FuelEntry);
            await _context.SaveChangesAsync();

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);
            if (vehicle != null && dto.Mileage > vehicle.Mileage)
            {
                vehicle.Mileage = dto.Mileage;
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<FuelEntryDto>(FuelEntry);
        }

        public async Task<List<FuelEntryDto>> GetAllFuelEntriesAsync()
        {
            var fuelEntries = await _context.FuelEntries.ToListAsync();
            return _mapper.Map<List<FuelEntryDto>>(fuelEntries);
        }

        public async Task<FuelEntryDto?> GetFuelEntryByIdAsync(int id)
        {
            var fuelEntry = await _context.FuelEntries.FirstOrDefaultAsync(le => le.FuelEntryId == id);
            return fuelEntry is null ? null : _mapper.Map<FuelEntryDto>(fuelEntry);
        }

        public async Task<FuelEntryDto?> UpdateFuelEntryByIdAsync(int id, UpdateFuelEntryDto dto)
        {
            var fuelEntry = await _context.FuelEntries.FirstOrDefaultAsync(le => le.FuelEntryId == id);
            if (fuelEntry is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.FuelType)) fuelEntry.FuelType = Enum.Parse<FuelType>(dto.FuelType, true);
            if (dto.RefillDate.HasValue) fuelEntry.RefillDate = dto.RefillDate.Value;
            if (dto.Amount.HasValue) fuelEntry.Amount = dto.Amount.Value;
            if (dto.Cost.HasValue) fuelEntry.Cost = dto.Cost.Value;
            if (dto.Mileage.HasValue)
            {
                fuelEntry.Mileage = dto.Mileage.Value;
                var vehicle = await _context.Vehicles.FindAsync(fuelEntry.VehicleId);
                if (vehicle != null && dto.Mileage.Value > vehicle.Mileage)
                    vehicle.Mileage = dto.Mileage.Value;
            }
            if (dto.Notes is not null) fuelEntry.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return _mapper.Map<FuelEntryDto>(fuelEntry);
        }

        public async Task<bool> DeleteFuelEntryByIdAsync(int id)
        {
            var fuelEntry = await _context.FuelEntries.FirstOrDefaultAsync(le => le.FuelEntryId == id);
            if (fuelEntry is null)
            {
                return false;
            }

            _context.FuelEntries.Remove(fuelEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FuelEntryDto>> GetByVehicleAsync(
            int vehicleId, string? fuelType, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.FuelEntries
                .Where(le => le.VehicleId == vehicleId);

            if (!string.IsNullOrEmpty(fuelType))
            {
                if (Enum.TryParse<FuelType>(fuelType, true, out var parsedFuelType))
                {
                    query = query.Where(le => le.FuelType == parsedFuelType);
                }
                else
                {
                    return [];
                }
            }

            if (fromDate.HasValue)
                query = query.Where(le => le.RefillDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(le => le.RefillDate <= toDate.Value);

            var entries = await query
                .OrderByDescending(le => le.RefillDate)
                .ToListAsync();

            return _mapper.Map<List<FuelEntryDto>>(entries);
        }
    }
}