namespace VehicleMaintenance.Models.Enums
{
    public enum ServiceType
    {
        Inspection,     // general checkup, MOT, annual service
        RoutineService, // oil change, filters, fluids, scheduled maintenance
        Repair,         // any unplanned fix — brakes, suspension, engine, etc.
        TyreService,    // rotation, swap, replacement, balancing, alignment
        BodyAndPaint,   // bodywork, paintwork, windshield, AC regas, wipers
        Electrical,     // battery, alternator, sensor replacement, diagnostics
        Other
    }
}
