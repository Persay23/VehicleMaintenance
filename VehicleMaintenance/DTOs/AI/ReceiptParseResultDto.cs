namespace VehicleMaintenance.DTOs.AI;

/// <summary>
/// Fields extracted from a receipt photo by Gemini, used to pre-fill the maintenance
/// record form. Doubles as the Gemini deserialisation target and the API response body —
/// nothing is persisted, so a single nullable shape serves both.
///
/// Every field is nullable with no validation attributes: a partial or malformed AI
/// response deserialises cleanly, and the user reviews/edits everything before submitting.
/// </summary>
public class ReceiptParseResultDto
{
    public string? ServiceName { get; set; }
    public DateTime? ServiceDate { get; set; }

    /// <summary>One of: Inspection, RoutineService, Repair, TyreService, BodyAndPaint, Electrical, Other.</summary>
    public string? ServiceType { get; set; }

    public int? Mileage { get; set; }
    public decimal? Cost { get; set; }

    /// <summary>ISO currency code detected on the receipt (e.g. UAH, PLN, USD, EUR) — null if unclear. Applies to Cost and all part costs.</summary>
    public string? Currency { get; set; }

    public string? VendorOrShop { get; set; }
    public string? TechnicianName { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Description { get; set; }

    /// <summary>
    /// Line-item parts/services found on the receipt. The frontend matches these against
    /// the vehicle's existing tracked components to pre-fill the record's component links.
    /// </summary>
    public List<ReceiptPartDto>? Parts { get; set; }
}

/// <summary>One line item from the receipt. All fields nullable — used only as suggestions.</summary>
public class ReceiptPartDto
{
    /// <summary>Human-readable part/work name, e.g. "Front brake pads".</summary>
    public string? Name { get; set; }

    /// <summary>Best-guess component category: Engine, Transmission, Brakes, Suspension, Electrical, Cooling, Fuel, Exhaust, Tyres, Body, Other — or null.</summary>
    public string? ComponentType { get; set; }

    /// <summary>What was done: Replaced, Repaired, Inspected, Adjusted, Cleaned, Other — or null.</summary>
    public string? ChangeType { get; set; }

    public decimal? PartsCost { get; set; }
    public string? WorkDescription { get; set; }
}
