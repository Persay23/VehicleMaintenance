using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class LiquidEntryService(AppDbContext context, IMapper mapper)
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

        public async Task<List<LiquidEntryDto>> GetLiquidEntriesByVehicleIdAsync(int vehicleId)
        {
            var liquidEntries = await _context.LiquidEntries
                .Where(le => le.VehicleId == vehicleId)
                .ToListAsync();

            return _mapper.Map<List<LiquidEntryDto>>(liquidEntries);
        }
    }
}