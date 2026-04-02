using PhotogramAPI.DTOs;

public interface IUserService
{
    Task<List<UserSearchDto>> SearchUsers(string query);
}
