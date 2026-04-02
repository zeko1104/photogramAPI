using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.DTOs;
using PhotogramAPI.Services;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("İstifadəçi tapılmadı.");

            
            var followerCount = await _context.Friendships.CountAsync(f => f.FollowingId == userId);
            var followingCount = await _context.Friendships.CountAsync(f => f.FollowerId == userId);

            var profile = new
            {
                user.Id,
                user.Username,
                user.Bio,
                user.ProfileImageUrl,
                PostCount = user.Posts.Count,
                FollowerCount = followerCount,
                FollowingCount = followingCount
            };

            return Ok(profile);
        }


        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(
        [FromForm] UpdateProfileDto dto,
        [FromServices] CloudinaryService cloudinary)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound("İstifadəçi tapılmadı.");

            user.Username = dto.Username;
            user.Bio = dto.Bio;

            if (dto.File != null)
            {
                
                if (!string.IsNullOrEmpty(user.ProfileImagePublicId))
                {
                    await cloudinary.DeleteImageAsync(user.ProfileImagePublicId);
                }

                
                var uploadResult = await cloudinary.UploadImageAsync(dto.File);
                user.ProfileImageUrl = uploadResult.Url;
                user.ProfileImagePublicId = uploadResult.PublicId;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profil yeniləndi",
                user.Username,
                user.Bio,
                user.ProfileImageUrl
            });
        }


    }
}
