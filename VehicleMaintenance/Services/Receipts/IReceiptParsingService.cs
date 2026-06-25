using VehicleMaintenance.DTOs.AI;

namespace VehicleMaintenance.Services.Receipts;

/// <summary>
/// Extracts form fields from a photographed document (receipt, invoice, label, reg paper) via Gemini.
/// Each method returns null if the AI call fails or returns nothing usable — callers fall back to manual entry.
/// </summary>
public interface IReceiptParsingService
{
    Task<ReceiptParseResultDto?>   ParseReceiptAsync(byte[] image, string mimeType, CancellationToken ct = default);
    Task<FuelParseResultDto?>      ParseFuelAsync(byte[] image, string mimeType, CancellationToken ct = default);
    Task<ComponentParseResultDto?> ParseComponentAsync(byte[] image, string mimeType, CancellationToken ct = default);
    Task<ExpenseParseResultDto?>   ParseExpenseAsync(byte[] image, string mimeType, CancellationToken ct = default);
    Task<VehicleParseResultDto?>   ParseVehicleAsync(byte[] image, string mimeType, CancellationToken ct = default);
}
