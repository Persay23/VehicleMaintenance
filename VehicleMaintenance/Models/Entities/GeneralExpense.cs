using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class GeneralExpense
    {
        public int GeneralExpenseId { get; private set; }
        public int VehicleId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime Date { get; set; }

        public bool IsRecurring { get; set; }
        public RecurrenceInterval? RecurrenceInterval { get; set; }
        public int? RecurrenceEvery { get; set; }
        public DateTime? RecurrenceEndDate { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
    }
}
