using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpadeUserAync(User user);
        Task DeleteUserAsync(int id);
    }
}
