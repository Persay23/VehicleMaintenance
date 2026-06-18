namespace VehicleMaintenance.DTOs.AI;

public class AiDiagnosisDto
{
    public int AiDiagnosisId { get; init; }
    public string Symptom { get; init; } = null!;
    public string Urgency { get; init; } = null!;
    public string UrgencyExplanation { get; init; } = null!;
    public List<string> LikelyCauses { get; init; } = [];
    public List<string> RecommendedActions { get; init; } = [];
    public List<string> RelatedComponents { get; init; } = [];
    public string Disclaimer { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}
