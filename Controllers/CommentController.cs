using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.DTOs;
using PhotogramAPI.Entities;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        
=======
        // 🟢 Yeni comment əlavə et
>>>>>>> 1db500ad90bed7aae5bb59f25b4600f053c4f99b
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var postExists = await _context.Posts.AnyAsync(p => p.Id == dto.PostId);
            if (!postExists) return NotFound("Post tapılmadı");

            var comment = new Comment
            {
                Content = dto.Content,
                PostId = dto.PostId,
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Şərh əlavə olundu", comment });
        }

<<<<<<< HEAD
        
=======
        // 🟡 Postun bütün şərhlərini gətir
>>>>>>> 1db500ad90bed7aae5bb59f25b4600f053c4f99b
        [AllowAnonymous]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Content,
                    c.CreatedAt,
                    Username = c.User.Username
                })
                .ToListAsync();

            return Ok(comments);
        }

<<<<<<< HEAD
        
=======
        // 🔴 Öz comment-ni sil
>>>>>>> 1db500ad90bed7aae5bb59f25b4600f053c4f99b
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (comment == null)
                return NotFound("Şərh tapılmadı və ya sənin deyil.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Şərh silindi" });
        }
    }
}
