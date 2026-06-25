using System.Text;
using System.Text.Json;

namespace VehicleMaintenance.Services.AI;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _model;

    private static readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["AiService:ApiKey"]!;
        _model = config["AiService:Model"] ?? "gemini-2.5-flash";
    }

    public async Task<string> AskAsync(string prompt, CancellationToken ct = default)
    {
        var body = new
        {
            contents = new[] { new { parts = new object[] { new { text = prompt } } } },
            generationConfig = new { responseMimeType = "application/json" }
        };

        return await GenerateAsync(body, ct);
    }

    public async Task<T?> AskJsonAsync<T>(string prompt, byte[] imageBytes, string mimeType, CancellationToken ct = default)
    {
        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = prompt },
                        new { inlineData = new { mimeType, data = Convert.ToBase64String(imageBytes) } }
                    }
                }
            },
            generationConfig = new { responseMimeType = "application/json" }
        };

        var raw = await GenerateAsync(body, ct);
        return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
    }

    private async Task<string> GenerateAsync(object body, CancellationToken ct)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta" +
                  $"/models/{_model}:generateContent?key={_apiKey}";

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync(url, content, ct);

            if (resp.IsSuccessStatusCode)
            {
                using var doc = await JsonDocument.ParseAsync(
                    await resp.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() == 0)
                    throw new InvalidOperationException(
                        "Gemini returned no candidates — the prompt was likely blocked by the safety filter.");

                return candidates[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString() ?? "";
            }

            if (attempt == 3 || (int)resp.StatusCode != 503)
            {
                var errorBody = await resp.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"Gemini {(int)resp.StatusCode}: {errorBody}");
            }

            await Task.Delay(TimeSpan.FromSeconds(attempt * 2), ct);
        }

        throw new InvalidOperationException("Gemini request failed after all retries.");
    }

    public async Task<T?> AskJsonAsync<T>(string prompt, CancellationToken ct = default)
    {
        var raw = await AskAsync(prompt, ct);
        return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
    }
}
