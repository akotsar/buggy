using System.Data.Entity;

namespace Buggy.Data.Initializers
{
    public class BuggyDbInitializer : CreateDatabaseIfNotExists<BuggyContext>
    {
        protected override void Seed(BuggyContext context)
        {
            base.Seed(context);

            var helper = new BuggyInitializerHelper(context);
            helper.CreateUsers().RunSynchronously();
            helper.CreateCars().RunSynchronously();
        }
    }
}