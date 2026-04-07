using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public int? DrivingExperience { get; set; }
    }
}
