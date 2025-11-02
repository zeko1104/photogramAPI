using PhotogramAPI.Entities;
using System.Threading.Tasks;

namespace PhotogramAPI.Services
{
    public interface IAuthService
    {
        Task<User> Register(string username, string password);
        Task<User?> Login(string username, string password);
        bool UserExists(string username);
        Task<string?> ChangePassword(int userId, string oldPassword, string newPassword);
    }
}
