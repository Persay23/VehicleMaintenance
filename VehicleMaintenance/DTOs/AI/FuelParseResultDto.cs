namespace VehicleMaintenance.DTOs.AI;

/// <summary>Fields extracted from a fuel / gas-station receipt. All nullable — the user reviews before saving.</summary>
public class FuelParseResultDto
{
    public string? Name { get; set; }       // station name, e.g. "Orlen"
    public string? Brand { get; set; }       // fuel brand if distinct from the station
    public string? FuelType { get; set; }    // Petrol95, Petrol98, Diesel, PremiumDiesel, LPG, CNG, Electric, Hydrogen, E85, Other
    public DateTime? RefillDate { get; set; }
    public decimal? Amount { get; set; }     // litres dispensed (kWh for an EV charge)
    public decimal? Cost { get; set; }
    public string? Currency { get; set; }    // ISO code of the amounts (UAH, PLN, USD, EUR…)
    public int? Mileage { get; set; }
    public string? Notes { get; set; }
}
