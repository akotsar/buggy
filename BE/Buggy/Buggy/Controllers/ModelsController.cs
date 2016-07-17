using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Data;
using Buggy.Dto;
using Buggy.Dto.Dashboard;
using Buggy.Extensions;
using Buggy.Models.Cars;
using Buggy.Models.Votes;

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
            IQueryable<Model> sourceQuery = _db.Models.Include("UserVotes").Include("Make");

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
                case "engine":
                    query = query.OrderBy(x => x.Model.EngineVol);
                    break;
                default:
                    query = query.OrderBy(x => x.Rank.ToString(CultureInfo.InvariantCulture));
                    break;
            }
            
            int skip = (page.GetValueOrDefault(1) - 1) * PageSize;
            query = query.Skip(skip).Take(PageSize);

            var totalPages = (int)Math.Ceiling((float)filtered.Count / PageSize);
            var models = query.Select(m => new ModelItem(m.Model) { Rank = m.Rank }).ToList();

            return new ModelsResponse { Models = models, TotalPages = totalPages };
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