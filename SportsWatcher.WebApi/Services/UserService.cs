using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            var userEntity = await userRepository.SingleOrDefaultAsync(x => x.Id == id);
            if (userEntity == null)
            {
                throw new Exception("User not found");
            }
            return UserDto.MapUserToUserDto(userEntity);
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.CreatedBy = "Platform";

            // Crypt the password
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

            await userRepository.AddAsync(user);
            await uow.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            var existingUser = await userRepository.SingleOrDefaultAsync(x => x.Id == updatedUser.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.UserFirstName = updatedUser.UserFirstName;
            existingUser.UserLastName = updatedUser.UserLastName;
            existingUser.PasswordHash = updatedUser.PasswordHash;
            existingUser.Country = updatedUser.Country;
            existingUser.UserEmail = updatedUser.UserEmail;
            existingUser.UserType = updatedUser.UserType;
            existingUser.UpdatedAt = DateTime.UtcNow;
            existingUser.CreatedBy = "Platform";

            userRepository.Update(existingUser);
            await uow.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                userRepository.Remove(user);
                await uow.SaveChangesAsync();
            }
        }
    }
}
