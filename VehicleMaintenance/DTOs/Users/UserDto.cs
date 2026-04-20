namespace VehicleMaintenance.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; } // e.g., male, female, other
        public int? DrivingExperience { get; set; }
    }
}
