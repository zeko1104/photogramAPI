using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.Dtos;
using System.Security.Claims;

namespace PhotogramAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FeedController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetFeed(int page = 1, int pageSize = 10)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var followingIds = await _context.Friendships
                .Where(f => f.FollowerId == currentUserId)
                .Select(f => f.FollowingId)
                .ToListAsync();

            followingIds.Add(currentUserId); 

            var posts = await _context.Posts
                .Where(p => followingIds.Contains(p.UserId))
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PostFeedDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            
            foreach (var post in posts)
            {
                post.IsLikedByCurrentUser = await _context.Likes
                    .AnyAsync(l => l.PostId == post.PostId && l.UserId == currentUserId);
            }

            return Ok(posts);
        }
    }
}
