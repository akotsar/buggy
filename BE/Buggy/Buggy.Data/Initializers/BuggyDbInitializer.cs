using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

using Buggy.Data.Seed;
using Buggy.Data.Users;
using Buggy.Models.Cars;
using Buggy.Models.Votes;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Newtonsoft.Json;

namespace Buggy.Data.Initializers
{
    public class BuggyDbInitializer : CreateDatabaseIfNotExists<BuggyContext>
    {
        protected override void Seed(BuggyContext context)
        {
            base.Seed(context);

            CreateUsers(context);
            CreateCars(context);
        }

        private void CreateUsers(BuggyContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (roleManager.FindByName("admin") == null)
            {
                roleManager.Create(new IdentityRole { Name = "admin" });
            }

            var userManager = new UserManager<User>(new UserStore<User>(context));
            var username = "admin";
            userManager.Create(
                new User { UserName = username, Email = username, FirstName = "Kate", LastName = "Nesmyelova" },
                "adminpass"); // Yeah, default password in open text. :(

            var admin = userManager.FindByName(username);
            if (!userManager.IsInRole(admin.Id, "admin"))
            {
                userManager.AddToRole(admin.Id, "admin");
            }
        }

        private void CreateCars(BuggyContext context)
        {
            var existingMakes = context.Makes.ToList();
            var existingModels = context.Models.ToList();

            var makes = ReadSeedData<Make[]>("Makes.json")
                .Where(x => existingMakes.All(e => e.Name != x.Name)).ToList();
            context.Makes.AddRange(makes);

            var models = ReadSeedData<SeedModel[]>("Models.json")
                .Where(x => existingModels.All(e => e.Name != x.Name || e.Make.Name != x.MakeName)).ToList();

            foreach (var model in models)
            {
                model.Make = makes.SingleOrDefault(x => x.Name == model.MakeName)
                    ?? existingMakes.Single(x => x.Name == model.MakeName);

                model.UserVotes =
                    (model.Comments ?? Enumerable.Empty<SeedComment>()).Select(
                        c =>
                        new UserVote
                        {
                            Comment = c.Comment,
                            DateVoted = c.DatePosted,
                            UserId = Guid.NewGuid().ToString()
                        }).ToList();

                context.Models.Add(model.ToModel());
            }

            context.SaveChanges();
        }

        private T ReadSeedData<T>(string name)
        {
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            var path = Path.Combine(
                Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)) ?? string.Empty,
                "Seed");

            using (StreamReader reader = new StreamReader(Path.Combine(path, name)))
            {
                string stringData = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(stringData);
            }
        }
    }
}