using SportsWatcher.WebApi.Enums;

namespace SportsWatcher.WebApi.Entities
{
    public class User : BaseEntity
    {
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        //TODO Add Requierd
        public string PasswordHash { get; set; }
        public string UserEmail { get; set; }   
        public UsersEnum UserType { get; set; } 

        //TODO Add Country/Region Nomenclature
    }
}
