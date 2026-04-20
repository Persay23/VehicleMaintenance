namespace VehicleMaintenance.Models.Enums
{
    public enum PredictionStatus
    {
        Active,
        Completed,   // auto-set when maintenance record is logged
        Ignored      // manually set by user
    }
}
