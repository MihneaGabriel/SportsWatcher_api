using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Services
{
    public class UserService(IGenericRepository<User> userRepository, IUnitOfWork uow) : IUserService
    {
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var userEntities = await userRepository.GetAllAsync();
            var userEntitiesDtos = new List<UserDto>();
            foreach (var u in userEntities)
            {
                userEntitiesDtos.Add(UserDto.MapUserToUserDto(u));
            }

            return userEntitiesDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var userEntity = await userRepository.SingleOrDefaultAsync(x => x.UserId == id);
            if (userEntity == null)
            {
                throw new Exception("User not found");
            }
            return UserDto.MapUserToUserDto(userEntity);
        }

        public async Task<User> AddUserAsync(User user)
        {
            await userRepository.AddAsync(user);
            await uow.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpadeUserAync(User user)
        {
            userRepository.Update(user);
            await uow.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await userRepository.FirstOrDefaultAsync(x => x.UserId == id);
            if (user != null)
            {
                userRepository.Remove(user);
                await uow.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
