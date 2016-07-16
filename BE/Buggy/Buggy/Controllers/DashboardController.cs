using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Data;
using Buggy.Dto.Dashboard;

namespace Buggy.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private readonly BuggyContext _db;

        public DashboardController(BuggyContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public async Task<Dashboard> GetDashboard()
        {
            var topMake =
                await
                (from make in _db.Makes
                 let votes = make.Models.Sum(m => m.Votes)
                 orderby votes descending 
                 select new { make, votes }).FirstAsync();

            var topModel =
                await
                (from model in _db.Models.Include(x => x.Make)
                 orderby model.Votes descending 
                 select model).FirstAsync();

            return new Dashboard
                   {
                       Make =
                           new DashboardMake
                           {
                               Id = topMake.make.Id,
                               Image = topMake.make.Image,
                               Name = topMake.make.Name,
                               Votes = topMake.votes
                           },
                       Model =
                           new DashboardModel
                           {
                               Id = topModel.Id,
                               Image = topModel.Image,
                               Make = topModel.Make.Name,
                               Name = topModel.Name,
                               Votes = topModel.Votes
                           }
                   };
        }
    }
}