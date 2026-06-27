using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using VehicleMaintenance.DTOs.Auth;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Auth;
using VehicleMaintenance.Services.Email;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService tokenService,
        IUserService iUserService,
        IEmailService emailService,
        IConfiguration config) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IJwtTokenService _tokenService = tokenService;
        private readonly IUserService _iUserService = iUserService;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _config = config;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var created = await _iUserService.CreateUserAsync(dto);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is not null)
                await SendConfirmationEmailAsync(user);

            return Ok(created);
        }

        [AllowAnonymous]
        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized(new { message = "Invalid email or password." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);

            // Correct password but the email isn't confirmed yet → a distinct, actionable message.
            if (result.IsNotAllowed)
                return Unauthorized(new { message = "Please confirm your email before logging in.", code = "email_not_confirmed" });

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = _tokenService.CreateToken(user, roles);

            return Ok(new { token, expiresAt });
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return BadRequest(new { message = "Invalid confirmation link." });

            string decoded;
            try { decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)); }
            catch { return BadRequest(new { message = "Invalid confirmation link." }); }

            var result = await _userManager.ConfirmEmailAsync(user, decoded);
            if (!result.Succeeded)
                return BadRequest(new { message = "Email confirmation failed or the link has expired." });

            return Ok(new { message = "Email confirmed — you can now log in." });
        }

        [AllowAnonymous]
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation(ResendConfirmationDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is not null && !await _userManager.IsEmailConfirmedAsync(user))
                await SendConfirmationEmailAsync(user);

            // Always generic — don't reveal whether the email exists or its confirmation state.
            return Ok(new { message = "If that address needs confirmation, a new link has been sent." });
        }

        // Stateless JWT: there is no server-side session to end — the client discards the token.
        [HttpPost("logout")]
        public IActionResult Logout() => Ok(new { message = "Logged out." });

        // ── helpers ──────────────────────────────────────────────────────────
        private async Task SendConfirmationEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var clientUrl = (_config["App:ClientUrl"] ?? "http://localhost:5173").TrimEnd('/');
            var link = $"{clientUrl}/confirm-email?userId={Uri.EscapeDataString(user.Id)}&token={encoded}";

            var html =
                $"""
                <p>Hi {WebUtility.HtmlEncode(user.Name)},</p>
                <p>Welcome to AutoCare! Confirm your email to activate your account:</p>
                <p><a href="{link}">Confirm my email</a></p>
                <p>If the button doesn't work, paste this link into your browser:<br>{link}</p>
                """;

            await _emailService.SendAsync(user.Email!, "Confirm your AutoCare account", html);
        }
    }
}
