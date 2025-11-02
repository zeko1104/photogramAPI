using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotogramAPI.DTOs;
using PhotogramAPI.Helpers;
using PhotogramAPI.Services;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(IAuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto dto)
        {
            if (_authService.UserExists(dto.Username))
                return BadRequest("Bu istifadəçi adı artıq mövcuddur");

            var user = await _authService.Register(dto.Username, dto.Password);
            var token = _tokenService.CreateToken(user);

            return Ok(token);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto dto)
        {
            var user = await _authService.Login(dto.Username, dto.Password);
            if (user == null) return Unauthorized("İstifadəçi adı və ya şifrə yanlışdır");

            var token = _tokenService.CreateToken(user);
            return Ok(token);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token contains no user id");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _authService.ChangePassword(userId, dto.OldPassword, dto.NewPassword);

            if (result == null)
                return NotFound("User not found.");

            if (result == "invalid_old_password")
                return BadRequest("Old password is incorrect.");

            if (result == "weak_password")
                return BadRequest("New password is too weak. It must be 8+ chars, include uppercase, lowercase, number, and symbol.");

            if (result == "same_password")
                return BadRequest("New password cannot be the same as the old password.");

            return Ok(new
            {
                message = "Password successfully changed.",
                newToken = result
            });
        }

    }
}
