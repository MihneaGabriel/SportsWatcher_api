using SportsWatcher.WebApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsWatcher.WebApi.Entities
{
    public class User : BaseEntity
    {
        public required string UserName { get; set; }
        public required string UserFirstName { get; set; }
        public required string UserLastName { get; set; }

        [MaxLength(100)]
        public required string PasswordHash { get; set; }
        public required string UserEmail { get; set; }
        public UsersEnum UserType { get; set; }
        public required string Country { get; set; }
    }
}
