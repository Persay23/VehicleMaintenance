namespace VehicleMaintenance.DTOs.AI
{
    public class DiagnoseResponseDto
    {
        public List<string> LikelyCauses       { get; init; } = [];
        public string       Urgency            { get; init; } = "safe";
        public string       UrgencyExplanation { get; init; } = "";
        public List<string> RecommendedActions { get; init; } = [];
        public List<string> RelatedComponents  { get; init; } = [];
        public string       Disclaimer         { get; init; } = "";
    }
}
