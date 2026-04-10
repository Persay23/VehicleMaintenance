using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class UserService(AppDbContext context, IMapper mapper, UserManager<User> userManager) : IUserService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<User> _userManager = userManager;
        // TODO: Add static general methods for creating, and retrieving users
        //// alot of repeted code in the controllers, better make a base controller for this? and alot of repetetive code in the services, better make a base service for this? 
        //////// also consider using automapper for mapping between entities and dtos, but for now we will do it manually
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email; // Identity requires UserName to be set

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserByIdAsync(string id, UpdateUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                return null;
            }

            if (dto.Name is not null) user.Name = dto.Name;  // is there a better way to do this
            if (dto.Email is not null) user.Email = dto.Email;
            if (dto.Age.HasValue) user.Age = dto.Age;
            if (dto.Gender.HasValue) user.Gender = dto.Gender.Value;
            if (dto.DrivingExperience.HasValue) user.DrivingExperience = dto.DrivingExperience;

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
