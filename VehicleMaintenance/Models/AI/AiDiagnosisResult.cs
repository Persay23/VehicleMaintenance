namespace VehicleMaintenance.Models.AI
{
    public record AiDiagnosisResult(
        List<string>? LikelyCauses       = null,
        string?       Urgency            = null,   // "safe" | "soon" | "stop"
        string?       UrgencyExplanation = null,
        List<string>? RecommendedActions = null,
        List<string>? RelatedComponents  = null,
        string?       Reasoning          = null    // dev/validation only — not returned to frontend
    );
}
