using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Repositories.Interfaces
{
    public interface IGeneralExpenseRepository
    {
        Task<GeneralExpense[]> GetGeneralExpensesByVehicleIdAsync(int vehicleId);
        Task<List<GeneralExpense>> GetGeneralExpensesByUserIdAsync(string userId);
        Task<GeneralExpense?> GetGeneralExpenseByIdAsync(int id);
        Task AddAsync(GeneralExpense expense);
        Task SaveAsync();
        Task<bool> DeleteGeneralExpenseAsync(int id);
    }
}
