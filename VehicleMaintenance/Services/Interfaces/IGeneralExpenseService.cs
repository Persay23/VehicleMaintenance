using VehicleMaintenance.DTOs.GeneralExpense;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IGeneralExpenseService
    {
        Task<GeneralExpenseDto[]> GetGeneralExpensesByVehicleIdAsync(int vehicleId);
        Task<List<GeneralExpenseDto>> GetGeneralExpensesByUserIdAsync(string userId);
        Task<GeneralExpenseDto?> GetGeneralExpenseByIdAsync(int id);
        Task<GeneralExpenseDto> CreateGeneralExpenseAsync(CreateGeneralExpenseDto dto);
        Task<GeneralExpenseDto?> UpdateGeneralExpenseAsync(int id, UpdateGeneralExpenseDto dto);
        Task<bool> DeleteGeneralExpenseAsync(int id);
    }
}
