namespace VehicleMaintenance.Services.AI;

public interface IGeminiService
{
    Task<string> AskAsync(string prompt, CancellationToken ct = default);
    Task<T?> AskJsonAsync<T>(string prompt, CancellationToken ct = default);
}
