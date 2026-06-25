namespace VehicleMaintenance.Services.AI;

public interface IGeminiService
{
    Task<string> AskAsync(string prompt, CancellationToken ct = default);
    Task<T?> AskJsonAsync<T>(string prompt, CancellationToken ct = default);

    /// <summary>
    /// Multimodal call: sends a text prompt alongside an image (e.g. a receipt photo)
    /// and deserialises the JSON response into <typeparamref name="T"/>.
    /// </summary>
    Task<T?> AskJsonAsync<T>(string prompt, byte[] imageBytes, string mimeType, CancellationToken ct = default);
}
