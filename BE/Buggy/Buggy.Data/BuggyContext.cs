using System.Data.Entity;
using Buggy.Data.Initializers;
using Buggy.Data.Users;
using Buggy.Models.Cars;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Buggy.Data
{
    public class BuggyContext : IdentityDbContext<User>
    {
        static BuggyContext()
        {
            Database.SetInitializer(new BuggyDbInitializer());
        }

        public BuggyContext()
            : base("BuggyDb")
        {
        }

        public virtual DbSet<Make> Makes { get; set; }

        public virtual DbSet<Model> Models { get; set; }
    }
}