namespace VehicleMaintenance.Models.AI
{
    /// <summary>
    /// All fields nullable — partial or malformed Gemini responses deserialise cleanly.
    /// Disclaimer is no longer requested from the model; the controller attaches it as a constant.
    /// Reasoning is captured for dev validation but not forwarded to the frontend.
    /// </summary>
    public record AiDiagnosisResult(
        List<string>? LikelyCauses       = null,
        string?       Urgency            = null,   // "safe" | "soon" | "stop"
        string?       UrgencyExplanation = null,
        List<string>? RecommendedActions = null,
        List<string>? RelatedComponents  = null,
        string?       Reasoning          = null    // dev/validation only — not returned to frontend
    );
}
