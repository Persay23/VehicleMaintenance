namespace VehicleMaintenance.Models.Enums
{
    public enum ComponentType
    {
        Engine,
        Transmission,
        Brakes,
        Suspension,
        Exhaust,
        Electrical,
        CoolingSystem,
        FuelSystem,
        Tires,
        Other // TODO: Must be an option for user to type by himself what kind of component it is, if it's not listed here
    }

    public enum State
    {
        New,
        Good,
        Normal,
        NeedsService,
        Critical,
        Unknown
    }
}
