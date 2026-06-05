using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Repositories.Interfaces
{
    public interface IUserDrivingProfileRepository
    {
        Task<UserDrivingProfile?> GetUserDrivingProfileByUserIdAsync(string userId);
        Task AddAsync(UserDrivingProfile profile);
        Task SaveAsync();
        Task<bool> DeleteUserDrivingProfileAsync(string userId);
    }
}
