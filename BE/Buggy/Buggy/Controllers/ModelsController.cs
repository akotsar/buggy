using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Data;
using Buggy.Dto;
using Buggy.Models.Cars;

namespace Buggy.Controllers
{
    [RoutePrefix("api/models")]
    public class ModelsController : ApiController
    {
        private static readonly int PageSize = 2;

        private readonly BuggyContext _db;

        public ModelsController(BuggyContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public async Task<ModelsResponse> GetModels(int? page = null, int? makeId = null, string orderBy = null)
        {
            IQueryable<Model> sourceQuery = _db.Models.Include("Comments").Include("Make");

            if (makeId.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.MakeId == makeId.Value);
            }

            var filtered = await sourceQuery.OrderByDescending(x => x.Votes).ToListAsync();

            var query = filtered
                .Select((x, i) => new { Model = x, Rank = i + 1 })
                .ToList()
                .AsQueryable();

            switch (orderBy?.ToLowerInvariant())
            {
                case "make":
                    query = query.OrderBy(x => x.Model.Make.Name);
                    break;
                case "name":
                    query = query.OrderBy(x => x.Model.Name);
                    break;
                case "votes":
                    query = query.OrderBy(x => x.Model.Votes);
                    break;
                default:
                    query = query.OrderBy(x => x.Rank);
                    break;
            }
            
            int skip = (page.GetValueOrDefault(1) - 1) * PageSize;
            query = query.Skip(skip).Take(PageSize);

            var totalPages = (int)Math.Ceiling((float)filtered.Count / PageSize);
            var models = query.Select(m => new ModelItem(m.Model) { Rank = m.Rank }).ToList();

            return new ModelsResponse { Models = models, TotalPages = totalPages };
        }
    }
}