using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Users
{
    public class UpdateUserDto
    {
        [MinLength(2)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int? Age { get; set; }
        public Gender? Gender { get; set; }
        public int? DrivingExperience { get; set; }
    }
}
