using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Auth;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Auth;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService tokenService,
        IUserService iUserService) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IJwtTokenService _tokenService = tokenService;
        private readonly IUserService _iUserService = iUserService;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var user = await _iUserService.CreateUserAsync(dto);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized(new { message = "Invalid email or password." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = _tokenService.CreateToken(user, roles);

            return Ok(new { token, expiresAt });
        }

        // Stateless JWT: there is no server-side session to end — the client discards the token.
        [HttpPost("logout")]
        public IActionResult Logout() => Ok(new { message = "Logged out." });
    }
}
