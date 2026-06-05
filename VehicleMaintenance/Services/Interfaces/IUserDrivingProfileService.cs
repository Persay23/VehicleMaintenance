using VehicleMaintenance.DTOs.UserDrivingProfile;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IUserDrivingProfileService
    {
        Task<UserDrivingProfileDto?> GetUserDrivingProfileByUserIdAsync(string userId);
        Task<UserDrivingProfileDto> CreateUserDrivingProfileAsync(CreateUserDrivingProfileDto dto);
        Task<UserDrivingProfileDto?> UpdateUserDrivingProfileAsync(string userId, UpdateUserDrivingProfileDto dto);
        Task<bool> DeleteUserDrivingProfileAsync(string userId);
    }
}
