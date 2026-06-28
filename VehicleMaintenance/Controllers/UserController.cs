using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.GenralModelService.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController(IUserService iUserService) : ControllerBase
    {
        private readonly IUserService _iUserService = iUserService;

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userDto = await _iUserService.GetCurrentUserAsync(User);
            if (userDto is null) return Unauthorized();
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var createdUser = await _iUserService.CreateUserAsync(createUserDto);
            return Ok(createdUser);
        }

        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto dto)
        {
            if (id != User.GetUserId()) return Forbid();

            var success = await _iUserService.ChangePasswordAsync(id, dto);
            if (!success) return BadRequest(new { message = "Password change failed." });
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserDto dto)
        {
            if (id != User.GetUserId()) return Forbid();

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
            if (id != User.GetUserId()) return Forbid();

            var deleted = await _iUserService.DeleteUserByIdAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
