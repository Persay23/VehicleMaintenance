using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.UserDrivingProfile;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDrivingProfileController(IUserDrivingProfileService service) : ControllerBase
    {
        private readonly IUserDrivingProfileService _service = service;

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDrivingProfileDto>> GetUserDrivingProfile(string userId)
        {
            if (userId != User.GetUserId()) return Forbid();

            var profile = await _service.GetUserDrivingProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<UserDrivingProfileDto>> CreateUserDrivingProfile(CreateUserDrivingProfileDto dto)
        {
            if (dto.UserId != User.GetUserId()) return Forbid();

            var created = await _service.CreateUserDrivingProfileAsync(dto);
            return CreatedAtAction(nameof(GetUserDrivingProfile), new { userId = created.UserId }, created);
        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult<UserDrivingProfileDto>> UpdateUserDrivingProfile(string userId, UpdateUserDrivingProfileDto dto)
        {
            if (userId != User.GetUserId()) return Forbid();

            var updated = await _service.UpdateUserDrivingProfileAsync(userId, dto);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserDrivingProfile(string userId)
        {
            if (userId != User.GetUserId()) return Forbid();

            var deleted = await _service.DeleteUserDrivingProfileAsync(userId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
