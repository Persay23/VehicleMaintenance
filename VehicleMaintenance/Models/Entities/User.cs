using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class User
    {
        public int UserId { get; private set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public int? DrivingExperience { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public ICollection<Vehicle> Vehicles { get; set; } = []; // icollection or list?
    }
}
