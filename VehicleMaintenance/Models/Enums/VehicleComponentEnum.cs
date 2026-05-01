namespace VehicleMaintenance.Models.Enums
{

    public enum ComponentType
    {
        Engine,         // spark plugs, timing belt, pistons, valves, oil
        Transmission,   // gearbox, clutch, flywheel, driveshaft, differential
        Brakes,         // pads, discs, calipers, brake fluid, ABS
        Suspension,     // shock absorbers, springs, ball joints, tie rods, bearings
        Electrical,     // battery, alternator, starter, fuses, sensors, lighting
        Cooling,        // radiator, water pump, thermostat, coolant, cooling fan
        Fuel,           // fuel pump, injectors, fuel filter, fuel lines
        Exhaust,        // exhaust pipe, catalytic converter, DPF, EGR, lambda sensor
        Tyres,          // tyres, wheels, TPMS, wheel alignment
        Body,           // windshield, wipers, mirrors, locks, AC, interior\
        Other
    }


    public enum State
    {
        Perfect,
        Good,
        Normal,
        Repair,
        Critical,
        Unknown
    }
}
