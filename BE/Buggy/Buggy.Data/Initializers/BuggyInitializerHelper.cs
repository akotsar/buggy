using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Buggy.Data.Seed;
using Buggy.Data.Users;
using Buggy.Models.Cars;
using Buggy.Models.Votes;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Newtonsoft.Json;

namespace Buggy.Data.Initializers
{
    public class BuggyInitializerHelper
    {
        private readonly BuggyContext _db;

        public BuggyInitializerHelper(BuggyContext db)
        {
            _db = db;
        }

        public async Task CreateUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            }

            var userManager = new UserManager<User>(new UserStore<User>(_db));
            var username = "admin";
            await userManager.CreateAsync(
                new User { UserName = username, Email = username, FirstName = "Kate", LastName = "Nesmyelova" },
                "adminpass"); // Yeah, default password in open text. :(

            var admin = await userManager.FindByNameAsync(username);
            if (!userManager.IsInRole(admin.Id, "admin"))
            {
                await userManager.AddToRoleAsync(admin.Id, "admin");
            }
        }

        public async Task ResetCars()
        {
            _db.Models.RemoveRange(_db.Models);
            _db.Makes.RemoveRange(_db.Makes);
            await _db.SaveChangesAsync();

            await CreateCars();
        }

        public async Task CreateCars()
        {
            var existingMakes = await _db.Makes.ToListAsync();
            var existingModels = await _db.Models.ToListAsync();

            var makes = ReadSeedData<Make[]>("Makes.json")
                .Where(x => existingMakes.All(e => e.Name != x.Name)).ToList();
            _db.Makes.AddRange(makes);

            var models = ReadSeedData<SeedModel[]>("Models.json")
                .Where(x => existingModels.All(e => e.Name != x.Name || e.Make.Name != x.MakeName)).ToList();

            foreach (var model in models)
            {
                model.Make = makes.SingleOrDefault(x => x.Name == model.MakeName)
                    ?? existingMakes.SingleOrDefault(x => x.Name == model.MakeName);

                if (model.Make == null)
                {
                    throw new InvalidOperationException($"Unable to find make '{model.MakeName}'.");
                }

                model.UserVotes =
                    (model.Comments ?? Enumerable.Empty<SeedComment>()).Select(
                        c =>
                        new UserVote
                        {
                            Comment = c.Comment,
                            DateVoted = c.DatePosted,
                            UserId = Guid.NewGuid().ToString()
                        }).ToList();

                _db.Models.Add(model.ToModel());
            }

            await _db.SaveChangesAsync();
        }

        private static T ReadSeedData<T>(string name)
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