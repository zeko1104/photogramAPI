using Microsoft.EntityFrameworkCore;
using PhotogramAPI.Data;
using PhotogramAPI.DTOs;
using PhotogramAPI.Entities;
using PhotogramAPI.Helpers;
using PhotogramAPI.Services;
using System.Security.Cryptography;
using System.Text;

namespace PhotogramAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService; 

        public AuthService(AppDbContext context, TokenService tokenService) 
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<User> Register(string username, string password)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower());
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return null;
            }

            return user;
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(x => x.Username == username.ToLower());
        }

        public Task<User> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<string?> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            if (!VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
                return "invalid_old_password";

            if (!PasswordValidator.IsStrongPassword(newPassword))
                return "weak_password";

            if (VerifyPasswordHash(newPassword, user.PasswordHash, user.PasswordSalt))
                return "same_password";

            CreatePasswordHash(newPassword, out byte[] newHash, out byte[] newSalt);
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            await _context.SaveChangesAsync();

            // ✅ token artıq işləyəcək
            var token = _tokenService.CreateToken(user);
            return token;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
    }
}
