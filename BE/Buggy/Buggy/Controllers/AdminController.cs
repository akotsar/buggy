using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Auth;
using Buggy.Data;
using Buggy.Data.Initializers;

namespace Buggy.Controllers
{
    [AdminAuthorize]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private readonly BuggyContext _db;

        public AdminController(BuggyContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("reset/cars")]
        public async Task ResetCars()
        {
            var helper = new BuggyInitializerHelper(_db);
            await helper.ResetCars();
        }
    }
}