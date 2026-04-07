using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class LiquidEntryService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<LiquidEntryDto> CreateLiquidEntryAsync(CreateLiquidEntryDto dto)
        {
            var liquidEntry = new LiquidEntry
            {
                VehicleId = dto.VehicleId,
                LiquidType = dto.LiquidType,
                RefillDate = dto.RefillDate,
                Amount = dto.Amount,
                Cost = dto.Cost,
                Mileage = dto.Mileage,
                Notes = dto.Notes
            };

            _context.LiquidEntries.Add(liquidEntry);
            await _context.SaveChangesAsync();

            return new LiquidEntryDto
            {
                LiquidEntryId = liquidEntry.LiquidEntryId,
                VehicleId = liquidEntry.VehicleId,
                LiquidType = dto.LiquidType,
                RefillDate = dto.RefillDate,
                Amount = dto.Amount,
                Cost = dto.Cost,
                Mileage = dto.Mileage,
                Notes = dto.Notes
            };
        }

        public async Task<List<LiquidEntryDto>> GetLiquidEntriesByVehicleIdAsync(int vehicleId)
        {
            return await _context.LiquidEntries
                .Where(le => le.VehicleId == vehicleId)
                .Select(le => new LiquidEntryDto
                {
                    LiquidEntryId = le.LiquidEntryId,
                    VehicleId = le.VehicleId,
                    LiquidType = le.LiquidType,
                    RefillDate = le.RefillDate,
                    Amount = le.Amount,
                    Cost = le.Cost,
                    Mileage = le.Mileage,
                    Notes = le.Notes
                })
                .ToListAsync();
        }
    }
}