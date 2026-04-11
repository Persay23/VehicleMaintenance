using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Services;


namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDto);
            return Ok(createdUser);
        }

        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto dto)
        {
            var success = await _userService.ChangePasswordAsync(id, dto);
            if (!success) return BadRequest(new { message = "Password change failed." });
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto dto)
        {
            var updated = await _userService.UpdateUserByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deleted = await _userService.DeleteUserByIdAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}
