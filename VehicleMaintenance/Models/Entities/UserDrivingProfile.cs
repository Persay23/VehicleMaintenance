using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class UserDrivingProfile
    {
        public int UserDrivingProfileId { get; private set; }
        public string UserId { get; set; } = null!;
        public int AnnualKm { get; set; }
        public PrimaryUsage PrimaryUsage { get; set; }
        public DrivingStyle DrivingStyle { get; set; }
        public UsagePattern UsagePattern { get; set; }
        public ClimateZone ClimateZone { get; set; }
        public ParkingType ParkingType { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
