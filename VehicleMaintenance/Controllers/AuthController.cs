using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Auth;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(SignInManager<User> signInManager, IUserService iUserService) : ControllerBase
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IUserService _iUserService = iUserService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var user = await _iUserService.CreateUserAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.Email, dto.Password,
                isPersistent: dto.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(new { message = "Logged in." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out." });
        }


    }
}