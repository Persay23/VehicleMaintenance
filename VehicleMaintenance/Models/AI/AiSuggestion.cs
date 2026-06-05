namespace VehicleMaintenance.Models.AI;

/// <summary>
/// All fields nullable — partial or malformed Gemini responses deserialise cleanly.
/// </summary>
public record AiSuggestion(
    string?   Title                = null,
    string?   Description         = null,
    string?   Urgency             = null,
    DateTime? SuggestedByDate     = null,
    int?      EstimatedRemainingKm = null,
    int?      VehicleComponentId  = null,  // ID from the component list in the prompt; null for vehicle-level suggestions
    double?   ConfidenceScore     = null   // 0.0–0.75; null if AI omits it
);
