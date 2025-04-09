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
        public string Country {  get; set; }

        //TODO Add Country/Region Nomenclature

        public static UserDto MapUserToUserDto(User userEntity)
        {
            return new UserDto
            {
                UserId = userEntity.Id,
                UserFirstName = userEntity.UserFirstName,
                PasswordHash = userEntity.PasswordHash,
                UserLastName = userEntity.UserLastName,
                UserEmail = userEntity.UserEmail,
                Country = userEntity.Country,
                UserType = userEntity.UserType,
            };
        }

        public static User MapUserDtoToUser(UserDto userDto)
        {
            return new User
            {
                Id = userDto.UserId,
                UserName = userDto.UserFirstName + userDto.UserLastName,
                UserFirstName = userDto.UserFirstName,
                UserLastName = userDto.UserLastName,
                PasswordHash = userDto.PasswordHash,
                UserEmail = userDto.UserEmail,
                Country= userDto.Country,
                UserType = userDto.UserType
            };
        }
    }
}
