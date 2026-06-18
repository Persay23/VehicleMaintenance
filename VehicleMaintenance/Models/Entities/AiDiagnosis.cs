namespace VehicleMaintenance.Models.Entities;

public class AiDiagnosis
{
    public int AiDiagnosisId { get; set; }
    public int VehicleId { get; set; }

    public string SymptomDescription { get; set; } = null!;

    public string Urgency { get; set; } = null!;            // Immediate | Soon | Monitor
    public string Summary { get; set; } = null!;
    public string LikelyCauses { get; set; } = null!;       // JSON string[]
    public string RecommendedActions { get; set; } = null!; // JSON string[]
    public string? RelatedComponentNames { get; set; }      // JSON string[] — nullable

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Vehicle Vehicle { get; set; } = null!;
}
