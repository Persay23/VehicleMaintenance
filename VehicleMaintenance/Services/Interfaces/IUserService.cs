using VehicleMaintenance.DTOs.Users;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> UpdateUserByIdAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteUserByIdAsync(int id);
    }
}
