using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Services.Interfaces;


namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController(IUserService iUserService) : ControllerBase
    {
        private readonly IUserService _iUserService = iUserService;

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _iUserService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _iUserService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var createdUser = await _iUserService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto dto)
        {
            var success = await _iUserService.ChangePasswordAsync(id, dto);
            if (!success) return BadRequest(new { message = "Password change failed." });
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto dto)
        {
            var updated = await _iUserService.UpdateUserByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deleted = await _iUserService.DeleteUserByIdAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}
