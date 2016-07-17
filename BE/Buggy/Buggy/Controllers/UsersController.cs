using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly BuggyUserManager _userManager;

        public UsersController(BuggyUserManager userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
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
            if (userId == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return new CurrentUserInfo { FirstName = user.FirstName, LastName = user.LastName };
        }

        [HttpGet]
        [Route("profile")]
        public async Task<UserProfile> GetProfile()
        {
            var userId = Request.GetUserId();
            if (userId == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(userId);
            return new UserProfile(user);
        }

        [HttpPut]
        [Route("profile")]
        public async Task UpdateProfile(UserProfile profile)
        {
            if (profile == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Profile data is missing."));
            }

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            // Injecting some bugs
            if (Regex.IsMatch(profile.Age ?? string.Empty, @"[^A-Za-z0-9]"))
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Get a candy ;)"));
            }

            if (profile.Hobby == "Knitting")
            {
                throw new InvalidOperationException("Knitting cannot be a hobby!");
            }

            if (profile.Gender?.Length > 10)
            {
                throw new InvalidOperationException("That's one weird gender!");
            }

            var userId = Request.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            // Checking password before any modification.
            if (!string.IsNullOrEmpty(profile.NewPassword))
            {
                if (await _userManager.FindAsync(user.UserName, profile.CurrentPassword ?? string.Empty) == null)
                {
                    throw new HttpResponseException(
                        Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Current password is incorrect. Unable to change password."));
                }
            }

            profile.UpdateUser(user);
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", res.Errors)));
            }

            // Changing password.
            if (!string.IsNullOrEmpty(profile.NewPassword))
            {
                res = await _userManager.ChangePasswordAsync(userId, profile.CurrentPassword, profile.NewPassword);
                if (!res.Succeeded)
                {
                    throw new HttpResponseException(
                        Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", res.Errors)));
                }
            }
        }
    }
}