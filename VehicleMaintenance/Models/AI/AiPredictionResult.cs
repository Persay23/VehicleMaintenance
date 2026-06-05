namespace VehicleMaintenance.Models.AI
{
    /// <summary>
    /// All fields are nullable so that a partial or malformed Gemini response
    /// deserialises cleanly instead of throwing JsonException.
    /// </summary>
    public record AiPredictionResult(
        DateTime? EstimatedNextServiceDate = null,
        int?      EstimatedRemainingKm     = null,
        double?   ConfidenceScore          = null,
        string?   Status                   = null,
        string?   Recommendation           = null,
        string?   Reasoning                = null,
        int?      HealthPercent            = null
    );
}
