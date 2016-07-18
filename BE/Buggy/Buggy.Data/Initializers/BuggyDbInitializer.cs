using System.Data.Entity;
using System.Threading.Tasks;

namespace Buggy.Data.Initializers
{
    public class BuggyDbInitializer : CreateDatabaseIfNotExists<BuggyContext>
    {
        protected override void Seed(BuggyContext context)
        {
            base.Seed(context);

            var helper = new BuggyInitializerHelper(context);
            Task.Factory.StartNew(() => helper.CreateUsers()).Unwrap().Wait();
            Task.Factory.StartNew(() => helper.CreateCars()).Unwrap().Wait();
        }
    }
}