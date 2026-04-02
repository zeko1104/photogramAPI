using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.Entities;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikeController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpPost("{postId}")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return NotFound("Post tapılmadı.");

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existingLike != null)
            {
                
                _context.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Like silindi (unliked)" });
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post bəyənildi (liked)" });
        }

        
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetLikeCount(int postId)
        {
            var count = await _context.Likes.CountAsync(l => l.PostId == postId);
            return Ok(new { postId, likeCount = count });
        }
    }
}
