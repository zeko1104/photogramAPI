using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.DTOs;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserSearchDto>> SearchUsers(string query)
    {
        return await _context.Users
            .Where(u => u.Username.Contains(query.ToLower()))
            .Select(u => new UserSearchDto
            {
                Id = u.Id,
                Username = u.Username,
                ProfileImageUrl = u.ProfileImageUrl
            })
            .ToListAsync();
    }
}
