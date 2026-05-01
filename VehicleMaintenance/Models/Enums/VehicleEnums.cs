namespace VehicleMaintenance.Models.Enums
{
    public enum VehicleType
    {
        // Cars
        Sedan,
        Hatchback,
        Estate,      // also called Wagon or Combi
        Coupe,
        Convertible,
        SUV,
        Crossover,
        MPV,         // Minivan / People Carrier

        // Working vehicles
        Pickup,
        Van,
        Truck,
        Bus,

        // Two-wheelers
        Motorcycle,
        Scooter,
        Moped,

        Other
    }

    public enum TransmissionType
    {
        Manual,
        Automatic,
        SemiAutomatic,
        CVT,          // Continuously Variable Transmission
        DCT,          // Dual Clutch Transmission
        Other
    }

    public enum EngineType
    {
        Petrol,
        Diesel,
        FullElectric,
        Hybrid,       // non-plug-in (self-charging)
        PlugInHybrid,
        Hydrogen,
        Other
    }

    public enum FuelType
    {
        Petrol95,
        Petrol98,
        Diesel,
        PremiumDiesel,
        LPG,          // Liquefied Petroleum Gas
        CNG,          // Compressed Natural Gas
        Electric,     // for EVs/charging events
        Hydrogen,
        E85,          // Bioethanol blend
        Other
    }

}