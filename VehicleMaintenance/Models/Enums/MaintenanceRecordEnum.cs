namespace VehicleMaintenance.Models.Enums
{
    public enum ServiceType
    {
        Brakes,
        Engine,
        Transmission,
        Tires,
        Other, // not sure about this enum options
        OilChange,
        Inspection,
        TyreSwap,
        BrakeService
        // i should add less options and devide them into groups like brakes, engine, transmission, tires, etc. so this field will serve as a tag for filtering and searching, and the service name will be the name of the service performed
    }
}
