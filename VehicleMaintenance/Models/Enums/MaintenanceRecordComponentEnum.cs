namespace VehicleMaintenance.Models.Enums
{
    public enum ComponentChangeType
    {
        Replaced, // when user select one of those changes, they should somehow impact the component, for example if user select replaced, then the old component should be marked as replaced and new component should be added to the database with the same name and brand
        Repaired, 
        Adjusted, 
        Inspected, 
        Cleaned, 
        Lubricated, 
        Other
    }
}
