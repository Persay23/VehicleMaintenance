using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Repositories.Interfaces;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Repositories
{
    public class GeneralExpenseRepository(AppDbContext context) : IGeneralExpenseRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<GeneralExpense>> GetGeneralExpensesByVehicleIdAsync(int vehicleId)
        {
            return await _context.GeneralExpenses
                .Where(e => e.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<List<GeneralExpense>> GetGeneralExpensesByUserIdAsync(string userId)
        {
            return await _context.GeneralExpenses
                .Where(e => e.Vehicle.UserId == userId)
                .ToListAsync();
        }

        public async Task<GeneralExpense?> GetGeneralExpenseByIdAsync(int id)
        {
            return await _context.GeneralExpenses
                .FirstOrDefaultAsync(e => e.GeneralExpenseId == id);
        }

        public async Task AddAsync(GeneralExpense expense)
        {
            await _context.GeneralExpenses.AddAsync(expense);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteGeneralExpenseAsync(int id)
        {
            var expense = await _context.GeneralExpenses
                .FirstOrDefaultAsync(e => e.GeneralExpenseId == id);

            if (expense is null)
                return false;

            _context.GeneralExpenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
