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
        // add liquids that were added to the vehicle as components, like oil, coolant, etc.
        Other, // TODO: Must be an option for user to type by himself what kind of component it is, if it's not listed here
        TimingBelt,
        Battery,
        AirFilter,
        EngineOil,
        BrakePads
    }

    public enum State
    {
        New,
        Good,
        Normal,
        NeedsService,
        Critical,
        Unknown,
        Warning,
        Monitor
    }
}
