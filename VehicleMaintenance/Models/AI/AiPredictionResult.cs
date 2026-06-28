namespace VehicleMaintenance.Models.AI
{
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
