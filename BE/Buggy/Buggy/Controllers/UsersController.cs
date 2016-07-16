using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Data.Identity;
using Buggy.Data.Users;
using Buggy.Dto;
using Buggy.Extensions;

using Microsoft.AspNet.Identity;

namespace Buggy.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UsersController : ApiController
    {
        private readonly BuggyUserManager _userManager;

        public UsersController(BuggyUserManager userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public void CreateUser(NewUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            var user = _userManager.FindByEmail(request.Username);
            if (user != null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User already exists."));
            }

            var result = _userManager.Create(
                new User { UserName = request.Username, Email = request.Username, FirstName = request.FirstName, LastName = request.LastName },
                request.Password);

            if (!result.Succeeded)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", result.Errors)));
            }
        }

        [HttpGet]
        [Route("current")]
        public async Task<CurrentUserInfo> GetCurrentUser()
        {
            var userId = Request.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return new CurrentUserInfo { FirstName = user.FirstName, LastName = user.LastName };
        }
    }
}