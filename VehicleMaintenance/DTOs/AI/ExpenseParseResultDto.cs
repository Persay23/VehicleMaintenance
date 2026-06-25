namespace VehicleMaintenance.DTOs.AI;

/// <summary>Fields extracted from a general expense receipt (insurance, parking, toll, etc.). All nullable — reviewed before saving.</summary>
public class ExpenseParseResultDto
{
    public string? ExpenseCategory { get; set; } // Insurance, Tax, Parking, Tolls, Fines, CarWash, Accessories, Other
    public decimal? Cost { get; set; }
    public string? Currency { get; set; }        // ISO code of the amount (UAH, PLN, USD, EUR…)
    public DateTime? Date { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
