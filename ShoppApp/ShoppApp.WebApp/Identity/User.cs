using Microsoft.AspNetCore.Identity;

namespace ShoppApp.WebApp.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
