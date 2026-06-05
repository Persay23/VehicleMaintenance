namespace VehicleMaintenance.Models.Enums
{
    public enum PrimaryUsage
    {
        City,
        Highway,
        Mixed,
        OffRoad,
        Track
    }

    public enum DrivingStyle
    {
        Gentle,
        Normal,
        Aggressive
    }

    public enum UsagePattern
    {
        Daily,
        WeekdaysOnly,
        WeekendsOnly,
        Occasional
    }

    public enum ClimateZone
    {
        Temperate,
        Cold,
        Hot,
        Humid
    }

    public enum ParkingType
    {
        Garage,
        Outdoor,
        Mixed
    }
}
