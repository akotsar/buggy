using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Data;
using Buggy.Dto;
using Buggy.Extensions;
using Buggy.Models.Cars;
using Buggy.Models.Votes;

namespace Buggy.Controllers
{
    [RoutePrefix("api/models")]
    public class ModelsController : ApiController
    {
        private static readonly int DefaultPageSize = 5;

        private readonly BuggyContext _db;

        public ModelsController(BuggyContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ModelsList GetModels(int? page = null, int? makeId = null, string orderBy = null, int? pageSize = null)
        {
            IQueryable<Model> models = _db.Models.Include("UserVotes").Include("Make");
            if (makeId.HasValue)
            {
                models = models.Where(x => x.MakeId == makeId.Value);
            }

            return new ModelsList(models, page, pageSize ?? DefaultPageSize, orderBy);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ModelDetails> GetModel(int id)
        {
            var model = await _db.Models.FindAsync(id);
            if (model == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = new ModelDetails(model, Request.GetUserId());

            var query = from vote in model.UserVotes
                        from user in _db.Users.Where(u => u.Id == vote.UserId).DefaultIfEmpty()
                        select new { Vote = vote, User = user };

            result.Comments = query
                .Where(x => !string.IsNullOrEmpty(x.Vote.Comment))
                .OrderByDescending(x => x.Vote.DateVoted)
                .Select(x => new Comment(x.Vote, x.User))
                .ToList();

            return result;
        }

        [HttpPost]
        [Route("{id}/vote")]
        [Authorize]
        public async Task Vote(int id, VoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            var model = await _db.Models.Include("UserVotes").SingleAsync(x => x.Id == id);
            var userId = Request.GetUserId();

            if (model.UserVotes.Any(x => x.UserId == userId))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot vote more than once"));
            }

            model.Votes++;
            model.UserVotes.Add(new UserVote
            {
                Comment = request.Comment,
                DateVoted = DateTime.UtcNow,
                UserId = Request.GetUserId()
            });

            await _db.SaveChangesAsync();
        }
    }
}