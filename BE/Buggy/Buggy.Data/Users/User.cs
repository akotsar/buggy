using Microsoft.AspNet.Identity.EntityFramework;

namespace Buggy.Data.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
