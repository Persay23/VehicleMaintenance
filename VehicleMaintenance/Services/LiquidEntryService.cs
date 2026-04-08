using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
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

            if (dto.LiquidType.HasValue) liquidEntry.LiquidType = dto.LiquidType.Value;
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
    }
}