using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.DTOs;
using PhotogramAPI.Entities;
using PhotogramAPI.Services;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public PostController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // 🟢 Yeni post yarat
        [HttpPost]
        [Consumes("multipart/form-data")] // ✅ şəkil gələcək deyə əlavə olundu
        public async Task<IActionResult> CreatePost([FromForm] PostDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return Unauthorized("İstifadəçi tapılmadı.");

            string? imageUrl = null;
            string? publicId = null;

            if (dto.Image != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Image);
                imageUrl = uploadResult.Url;
                publicId = uploadResult.PublicId;
            }

            var post = new Post
            {
                Content = dto.Content,
                ImageUrl = imageUrl,
                ImagePublicId = publicId,
                UserId = user.Id
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post yaradıldı", post });
        }

        // 🟡 Bütün postları gətir
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.Content,
                    p.ImageUrl,
                    p.CreatedAt,
                    Username = p.User.Username
                })
                .ToListAsync();

            return Ok(posts);
        }

        // 🔴 Öz postunu sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            // Token-dən user.Id götürürük
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized("İstifadəçi tapılmadı.");

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);
            if (post == null)
                return NotFound("Post tapılmadı və ya sənin deyil.");

            if (!string.IsNullOrEmpty(post.ImagePublicId))
                await _cloudinaryService.DeleteImageAsync(post.ImagePublicId);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post silindi" });
        }

    }
}
