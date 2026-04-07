using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Services.Security;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services
{
    public class UserService(AppDbContext context, IPasswordHasher passwordHasher)
    {
        private readonly AppDbContext _context = context;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age,
                Gender = dto.Gender,
                DrivingExperience = dto.DrivingExperience,
                PasswordHash = _passwordHasher.Hash(dto.Password )
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.UserId,
                Name = user.Name,
                Email = dto.Email,
                Age = dto.Age,
                Gender = dto.Gender,
                DrivingExperience = dto.DrivingExperience
            };
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Age = u.Age,
                    Gender = u.Gender,
                    DrivingExperience = u.DrivingExperience
                })
                .ToListAsync();
        }
    }
}
