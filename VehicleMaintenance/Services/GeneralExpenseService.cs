using AutoMapper;
using VehicleMaintenance.DTOs.GeneralExpense;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Repositories.Interfaces;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class GeneralExpenseService(IGeneralExpenseRepository repository, IMapper mapper) : IGeneralExpenseService
    {
        private readonly IGeneralExpenseRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<GeneralExpenseDto[]> GetGeneralExpensesByVehicleIdAsync(int vehicleId)
        {
            var expenses = await _repository.GetGeneralExpensesByVehicleIdAsync(vehicleId);
            return [.. expenses.Select(MapToDto)];
        }

        public async Task<List<GeneralExpenseDto>> GetGeneralExpensesByUserIdAsync(string userId)
        {
            var expenses = await _repository.GetGeneralExpensesByUserIdAsync(userId);
            return [.. expenses.Select(MapToDto)];
        }

        public async Task<GeneralExpenseDto?> GetGeneralExpenseByIdAsync(int id)
        {
            var expense = await _repository.GetGeneralExpenseByIdAsync(id);
            return expense is null ? null : MapToDto(expense);
        }

        public async Task<GeneralExpenseDto> CreateGeneralExpenseAsync(CreateGeneralExpenseDto dto)
        {
            var expense = _mapper.Map<GeneralExpense>(dto);
            await _repository.AddAsync(expense);
            await _repository.SaveAsync();
            return MapToDto(expense);
        }

        public async Task<GeneralExpenseDto?> UpdateGeneralExpenseAsync(int id, UpdateGeneralExpenseDto dto)
        {
            var expense = await _repository.GetGeneralExpenseByIdAsync(id);
            if (expense is null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.ExpenseCategory))
                expense.ExpenseCategory = Enum.Parse<ExpenseCategory>(dto.ExpenseCategory, true);
            if (dto.Cost.HasValue) expense.Cost = dto.Cost.Value;
            if (dto.Description is not null) expense.Description = dto.Description;
            if (dto.Notes is not null) expense.Notes = dto.Notes;
            if (dto.Date.HasValue) expense.Date = dto.Date.Value;
            if (dto.IsRecurring.HasValue) expense.IsRecurring = dto.IsRecurring.Value;
            if (!string.IsNullOrWhiteSpace(dto.RecurrenceInterval))
                expense.RecurrenceInterval = Enum.Parse<RecurrenceInterval>(dto.RecurrenceInterval, true);
            if (dto.RecurrenceEvery.HasValue) expense.RecurrenceEvery = dto.RecurrenceEvery.Value;
            if (dto.RecurrenceEndDate.HasValue) expense.RecurrenceEndDate = dto.RecurrenceEndDate.Value;

            await _repository.SaveAsync();
            return MapToDto(expense);
        }

        public async Task<bool> DeleteGeneralExpenseAsync(int id)
        {
            return await _repository.DeleteGeneralExpenseAsync(id);
        }

        private static GeneralExpenseDto MapToDto(GeneralExpense expense)
        {
            return new GeneralExpenseDto
            {
                GeneralExpenseId   = expense.GeneralExpenseId,
                VehicleId          = expense.VehicleId,
                ExpenseCategory    = expense.ExpenseCategory.ToString(),
                Cost               = expense.Cost,
                Description        = expense.Description,
                Notes              = expense.Notes,
                Date               = expense.Date,
                IsRecurring        = expense.IsRecurring,
                RecurrenceInterval = expense.RecurrenceInterval?.ToString(),
                RecurrenceEvery    = expense.RecurrenceEvery,
                RecurrenceEndDate  = expense.RecurrenceEndDate,
                NextOccurrenceDate = ComputeNextOccurrence(expense),
            };
        }

        private static DateTime? ComputeNextOccurrence(GeneralExpense expense)
        {
            if (!expense.IsRecurring || expense.RecurrenceInterval is null || expense.RecurrenceEvery is null)
                return null;

            var next = expense.Date;
            var now  = DateTime.UtcNow;

            while (next <= now)
            {
                next = expense.RecurrenceInterval switch
                {
                    RecurrenceInterval.Days   => next.AddDays(expense.RecurrenceEvery.Value),
                    RecurrenceInterval.Weeks  => next.AddDays(expense.RecurrenceEvery.Value * 7),
                    RecurrenceInterval.Months => next.AddMonths(expense.RecurrenceEvery.Value),
                    RecurrenceInterval.Years  => next.AddYears(expense.RecurrenceEvery.Value),
                    _                         => next,
                };
            }

            if (expense.RecurrenceEndDate.HasValue && next > expense.RecurrenceEndDate.Value)
                return null;

            return next;
        }
    }
}
