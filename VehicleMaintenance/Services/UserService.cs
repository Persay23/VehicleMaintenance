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
        // TODO: Add static general methods for creating, and retrieving users
        //// alot of repeted code in the controllers, better make a base controller for this? and alot of repetetive code in the services, better make a base service for this? 
        //////// also consider using automapper for mapping between entities and dtos, but for now we will do it manually
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
