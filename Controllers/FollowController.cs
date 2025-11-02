using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.Entities;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FollowController : ControllerBase
{
    private readonly AppDbContext _context;

    public FollowController(AppDbContext context)
    {
        _context = context;
    }

    // 🟢 Follow / Unfollow
    [HttpPost("{userId}")]
    public async Task<IActionResult> FollowUnfollow(int userId)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (currentUserId == userId)
            return BadRequest("Özünü izləyə bilməzsən.");

        var existing = await _context.Friendships
            .FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowingId == userId);

        if (existing != null)
        {
            // Already following → Unfollow
            _context.Friendships.Remove(existing);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User-un follow-u ləğv edildi (unfollowed)" });
        }

        // Yeni follow
        var friendship = new Friendship
        {
            FollowerId = currentUserId,
            FollowingId = userId
        };

        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User izlənildi (followed)" });
    }

    // 🟡 İzlədiklərimi gətir
    [HttpGet("following")]
    public async Task<IActionResult> GetFollowing()
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var following = await _context.Friendships
            .Where(f => f.FollowerId == currentUserId)
            .Include(f => f.Following)
            .Select(f => new { f.Following.Id, f.Following.Username })
            .ToListAsync();

        return Ok(following);
    }

    // 🟡 Məni izləyənləri gətir
    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowers()
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var followers = await _context.Friendships
            .Where(f => f.FollowingId == currentUserId)
            .Include(f => f.Follower)
            .Select(f => new { f.Follower.Id, f.Follower.Username })
            .ToListAsync();

        return Ok(followers);
    }
}
