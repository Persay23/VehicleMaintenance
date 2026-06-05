namespace VehicleMaintenance.DTOs.UserDrivingProfile
{
    public class CreateUserDrivingProfileDto
    {
        public string UserId { get; set; } = null!;
        public int AnnualKm { get; set; }
        public string PrimaryUsage { get; set; } = null!;
        public string DrivingStyle { get; set; } = null!;
        public string UsagePattern { get; set; } = null!;
        public string ClimateZone { get; set; } = null!;
        public string ParkingType { get; set; } = null!;
    }
}
