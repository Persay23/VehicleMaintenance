using VehicleMaintenance.DTOs.Users;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto?> UpdateUserByIdAsync(string id, UpdateUserDto dto);
        Task<bool> DeleteUserByIdAsync(string id);
    }
}
