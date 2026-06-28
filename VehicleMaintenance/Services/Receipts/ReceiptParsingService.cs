using VehicleMaintenance.DTOs.AI;
using VehicleMaintenance.Services.AI;

namespace VehicleMaintenance.Services.Receipts;

public class ReceiptParsingService(IGeminiService gemini, ILogger<ReceiptParsingService> logger)
    : IReceiptParsingService
{
    private readonly IGeminiService _gemini = gemini;
    private readonly ILogger<ReceiptParsingService> _logger = logger;

    public Task<ReceiptParseResultDto?> ParseReceiptAsync(byte[] image, string mimeType, CancellationToken ct = default)
        => ParseAsync<ReceiptParseResultDto>(PromptBuilderService.BuildReceiptParsePrompt(), image, mimeType, ct);

    public Task<FuelParseResultDto?> ParseFuelAsync(byte[] image, string mimeType, CancellationToken ct = default)
        => ParseAsync<FuelParseResultDto>(PromptBuilderService.BuildFuelParsePrompt(), image, mimeType, ct);

    public Task<ComponentParseResultDto?> ParseComponentAsync(byte[] image, string mimeType, CancellationToken ct = default)
        => ParseAsync<ComponentParseResultDto>(PromptBuilderService.BuildComponentParsePrompt(), image, mimeType, ct);

    public Task<ExpenseParseResultDto?> ParseExpenseAsync(byte[] image, string mimeType, CancellationToken ct = default)
        => ParseAsync<ExpenseParseResultDto>(PromptBuilderService.BuildExpenseParsePrompt(), image, mimeType, ct);

    public Task<VehicleParseResultDto?> ParseVehicleAsync(byte[] image, string mimeType, CancellationToken ct = default)
        => ParseAsync<VehicleParseResultDto>(PromptBuilderService.BuildVehicleParsePrompt(), image, mimeType, ct);

    private async Task<T?> ParseAsync<T>(string prompt, byte[] image, string mimeType, CancellationToken ct) where T : class // deep into
    {
        try
        {
            return await _gemini.AskJsonAsync<T>(prompt, image, mimeType, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Document parsing failed for {Type}; falling back to manual entry.", typeof(T).Name);
            return null;
        }
    }
}
