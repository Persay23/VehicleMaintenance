using VehicleMaintenance.DTOs.Users;

namespace VehicleMaintenance.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> PostAsync(CreateUserDto dto);
        Task<UserDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<UserDto?> PatchAsync(int id, UpdateUserDto dto);
    }
}
