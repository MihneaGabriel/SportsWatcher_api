using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Enums;

namespace SportsWatcher.WebApi.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        //TODO Add Requierd
        public string PasswordHash { get; set; }
        public string UserEmail { get; set; }
        public UsersEnum UserType { get; set; }

        //TODO Add Country/Region Nomenclature

        public static UserDto MapUserToUserDto(User userEntity)
        {
            return new UserDto
            {
                UserId = userEntity.UserId,
                UserFirstName = userEntity.UserFirstName,
                UserLastName = userEntity.UserLastName,
                UserEmail = userEntity.UserEmail,
                UserType = userEntity.UserType,
            };
        }
    }
}
