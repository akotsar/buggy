using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Buggy.Data.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public int? Age { get; set; }

        public string Address { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        public string Hobby { get; set; }
    }
}
