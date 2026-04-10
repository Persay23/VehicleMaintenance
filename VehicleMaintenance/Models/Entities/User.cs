using Microsoft.AspNetCore.Identity;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public int? DrivingExperience { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public List<Vehicle> Vehicles { get; set; } = [];
    }
}
