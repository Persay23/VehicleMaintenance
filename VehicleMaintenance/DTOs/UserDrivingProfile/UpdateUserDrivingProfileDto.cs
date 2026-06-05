namespace VehicleMaintenance.DTOs.UserDrivingProfile
{
    public class UpdateUserDrivingProfileDto
    {
        public int? AnnualKm { get; set; }
        public string? PrimaryUsage { get; set; }
        public string? DrivingStyle { get; set; }
        public string? UsagePattern { get; set; }
        public string? ClimateZone { get; set; }
        public string? ParkingType { get; set; }
    }
}
