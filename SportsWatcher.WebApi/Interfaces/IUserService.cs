using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserAsync(LoginDto login);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
