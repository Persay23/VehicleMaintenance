using System.Security.Claims;
using VehicleMaintenance.DTOs.Users;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto[]> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto?> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto?> UpdateUserByIdAsync(string id, UpdateUserDto dto);
        Task<bool> ChangePasswordAsync(string id, ChangePasswordDto dto);
        Task<bool> DeleteUserByIdAsync(string id);
    }
}
