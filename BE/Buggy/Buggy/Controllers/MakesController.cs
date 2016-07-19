using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Buggy.Data;
using Buggy.Dto;
using Buggy.Extensions;

namespace Buggy.Controllers
{
    [RoutePrefix("api/makes")]
    public class MakesController : ApiController
    {
        private static readonly int DefaultPageSize = 5;

        private readonly BuggyContext _db;

        public MakesController(BuggyContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<MakeDetails> GetModel(int id, int? modelsPage = null, int? modelsPageSize = null, string modelsOrderBy = null)
        {
            var make = await _db.Makes.FindAsync(id);
            if (make == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = new MakeDetails(make, modelsPage, modelsPageSize ?? DefaultPageSize, modelsOrderBy);
            return result;
        }
    }
}