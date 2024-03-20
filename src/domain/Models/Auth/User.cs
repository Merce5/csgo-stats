using Microsoft.AspNetCore.Identity;

namespace domain.Models.Auth
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}