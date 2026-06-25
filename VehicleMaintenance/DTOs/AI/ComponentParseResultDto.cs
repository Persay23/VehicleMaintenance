namespace VehicleMaintenance.DTOs.AI;

/// <summary>Fields extracted from a parts invoice or product/box label. All nullable — the user reviews before saving.</summary>
public class ComponentParseResultDto
{
    public string? ComponentType { get; set; }         // Engine, Transmission, Brakes, Suspension, Electrical, Cooling, Fuel, Exhaust, Tyres, Body, Other
    public string? VehicleComponentName { get; set; }
    public string? VehicleComponentBrand { get; set; }
    public string? PartNumber { get; set; }
    public DateTime? InstallationDate { get; set; }    // often the purchase date on the receipt
    public int? ExpectedLifetimeKm { get; set; }
    public int? ExpectedLifetimeYears { get; set; }
    public int? WarrantyKm { get; set; }
    public DateTime? WarrantyDate { get; set; }
    public string? Notes { get; set; }
}
