using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class LiquidEntryService(AppDbContext context, IMapper mapper) : ILiquidEntryService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<LiquidEntryDto> CreateLiquidEntryAsync(CreateLiquidEntryDto dto)
        {
            var liquidEntry = _mapper.Map<LiquidEntry>(dto);

            _context.LiquidEntries.Add(liquidEntry);
            await _context.SaveChangesAsync();

            return _mapper.Map<LiquidEntryDto>(liquidEntry);
        }

        public async Task<List<LiquidEntryDto>> GetAllLiquidEntriesAsync()
        {
            var liquidEntries = await _context.LiquidEntries.ToListAsync();
            return _mapper.Map<List<LiquidEntryDto>>(liquidEntries);
        }

        public async Task<LiquidEntryDto?> GetLiquidEntryByIdAsync(int id)
        {
            var liquidEntry = await _context.LiquidEntries.FirstOrDefaultAsync(le => le.LiquidEntryId == id);
            return liquidEntry is null ? null : _mapper.Map<LiquidEntryDto>(liquidEntry);
        }

        public async Task<LiquidEntryDto?> UpdateLiquidEntryByIdAsync(int id, UpdateLiquidEntryDto dto)
        {
            var liquidEntry = await _context.LiquidEntries.FirstOrDefaultAsync(le => le.LiquidEntryId == id);
            if (liquidEntry is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.LiquidType)) liquidEntry.LiquidType = Enum.Parse<LiquidType>(dto.LiquidType, true);
            if (dto.RefillDate.HasValue) liquidEntry.RefillDate = dto.RefillDate.Value;
            if (dto.Amount.HasValue) liquidEntry.Amount = dto.Amount.Value;
            if (dto.Cost.HasValue) liquidEntry.Cost = dto.Cost.Value;
            if (dto.Mileage.HasValue) liquidEntry.Mileage = dto.Mileage.Value;
            if (dto.Notes is not null) liquidEntry.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return _mapper.Map<LiquidEntryDto>(liquidEntry);
        }

        public async Task<bool> DeleteLiquidEntryByIdAsync(int id)
        {
            var liquidEntry = await _context.LiquidEntries.FirstOrDefaultAsync(le => le.LiquidEntryId == id);
            if (liquidEntry is null)
            {
                return false;
            }

            _context.LiquidEntries.Remove(liquidEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<LiquidEntryDto>> GetByVehicleAsync(
    int vehicleId, LiquidType? liquidType, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.LiquidEntries
                .Where(le => le.VehicleId == vehicleId);

            // LiquidType is an enum on your entity
            if (liquidType.HasValue)
                query = query.Where(le => le.LiquidType == liquidType.Value);

            // RefillDate is DateTime on your entity
            if (fromDate.HasValue)
                query = query.Where(le => le.RefillDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(le => le.RefillDate <= toDate.Value);

            var entries = await query
                .OrderByDescending(le => le.RefillDate)
                .ToListAsync();

            return _mapper.Map<List<LiquidEntryDto>>(entries);
        }
    }
}