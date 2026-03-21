using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models;

namespace VehicleMaintenance.DTOs.Users
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public int? Age { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public int? DrivingExperience { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
