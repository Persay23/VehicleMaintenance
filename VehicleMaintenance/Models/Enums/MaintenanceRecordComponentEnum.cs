namespace VehicleMaintenance.Models.Enums
{
    public enum ComponentChangeType
    {
        Replaced, // as new(perfect)
        Repaired, // as old but good
        Adjusted, // 
        Inspected, // the same state
        Cleaned, // +10% to health
        Lubricated, 
        Other
    }
}
