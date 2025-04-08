using SportsWatcher.WebApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsWatcher.WebApi.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }
        public string UserEmail { get; set; }
        public UsersEnum UserType { get; set; }
        public string Country { get; set; }
    }
}
