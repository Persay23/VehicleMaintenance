namespace VehicleMaintenance.DTOs.AI;

/// <summary>Fields extracted from a vehicle registration document / spec sheet. All nullable — reviewed before saving.</summary>
public class VehicleParseResultDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? YearOfProduction { get; set; }
    public string? VehicleType { get; set; }       // Sedan, Hatchback, Estate, Coupe, Convertible, SUV, Crossover, MPV, Pickup, Van, Truck, Bus, Motorcycle, Scooter, Moped, Other
    public string? TransmissionType { get; set; }  // Manual, Automatic, SemiAutomatic, CVT, DCT, Other
    public string? EngineType { get; set; }        // Petrol, Diesel, FullElectric, Hybrid, PlugInHybrid, Hydrogen, Other
    public string? FuelType { get; set; }          // Petrol95, Petrol98, Diesel, PremiumDiesel, LPG, CNG, Electric, Hydrogen, E85, Other
    public int? Mileage { get; set; }
}
