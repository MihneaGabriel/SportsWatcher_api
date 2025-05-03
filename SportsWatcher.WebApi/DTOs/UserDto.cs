using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Enums;

namespace SportsWatcher.WebApi.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UsersEnum UserType { get; set; }
        public string Country { get; set; }
        public Boolean TermsAgreement { get; set; } = false;
        public Boolean AgeConfirmation { get; set; } = false;
        public Boolean Promotions { get; set; } = false; // TODO 

        public static UserDto MapUserToUserDto(User userEntity)
        {
            return new UserDto
            {
                UserId = userEntity.Id,
                UserName = userEntity.UserName,
                Password = userEntity.PasswordHash,
                Email = userEntity.UserEmail,
                Country = userEntity.Country,
                UserType = userEntity.UserType,
            };
        }

        public static User MapUserDtoToUser(UserDto userDto)
        {
            var nameParts = string.IsNullOrWhiteSpace(userDto.UserName) ? [] : userDto.UserName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return new User
            {
                Id = userDto.UserId,
                UserName = userDto.UserName,
                UserFirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                UserLastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
                PasswordHash = userDto.Password,
                UserEmail = userDto.Email,
                Country = userDto.Country,
                UserType = userDto.UserType
            };
        }
    }
}
