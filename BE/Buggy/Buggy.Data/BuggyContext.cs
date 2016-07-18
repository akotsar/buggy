using System.Data.Entity;
using Buggy.Data.Initializers;
using Buggy.Data.Users;
using Buggy.Models.Cars;
using Buggy.Models.Votes;

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

        public virtual DbSet<UserVote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model>()
                .HasRequired(model => model.Make)
                .WithMany(make => make.Models)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserVote>()
                .HasRequired(vote => vote.Model)
                .WithMany(model => model.UserVotes)
                .WillCascadeOnDelete();
        }
    }
}