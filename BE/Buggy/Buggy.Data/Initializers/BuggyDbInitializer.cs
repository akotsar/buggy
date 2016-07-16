using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

using Buggy.Data.Seed;
using Buggy.Data.Users;
using Buggy.Models.Cars;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Newtonsoft.Json;

namespace Buggy.Data.Initializers
{
    public class BuggyDbInitializer : DropCreateDatabaseAlways<BuggyContext>
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
            var makes = ReadSeedData<Make[]>("Makes.json");
            context.Makes.AddRange(makes);

            var models = ReadSeedData<SeedModel[]>("Models.json");
            foreach (var model in models)
            {
                model.Make = makes.Single(x => x.Name == model.MakeName);
            }

            context.Models.AddRange(models);

            context.SaveChanges();
        }

        private T ReadSeedData<T>(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Buggy.Data.Seed." + name;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string stringData = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(stringData);
            }
        }
    }
}
