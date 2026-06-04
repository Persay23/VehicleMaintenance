namespace VehicleMaintenance.DTOs.GeneralExpense
{
    public class UpdateGeneralExpenseDto
    {
        public string? ExpenseCategory { get; set; }
        public decimal? Cost { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }

        public bool? IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public int? RecurrenceEvery { get; set; }
        public DateTime? RecurrenceEndDate { get; set; }
    }
}
