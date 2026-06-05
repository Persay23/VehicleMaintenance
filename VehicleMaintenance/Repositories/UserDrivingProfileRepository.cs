using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Repositories.Interfaces;

namespace VehicleMaintenance.Repositories
{
    public class UserDrivingProfileRepository(AppDbContext context) : IUserDrivingProfileRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<UserDrivingProfile?> GetUserDrivingProfileByUserIdAsync(string userId)
        {
            return await _context.UserDrivingProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(UserDrivingProfile profile)
        {
            await _context.UserDrivingProfiles.AddAsync(profile);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserDrivingProfileAsync(string userId)
        {
            var profile = await _context.UserDrivingProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile is null)
                return false;

            _context.UserDrivingProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
