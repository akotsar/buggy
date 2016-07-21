using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Buggy.Auth;
using Buggy.Data;
using Buggy.Data.Identity;
using Buggy.Data.Initializers;
using Buggy.Data.Users;
using Buggy.Dto;

namespace Buggy.Controllers
{
    [AdminAuthorize]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private static readonly string[] PlainPasswords;

        static AdminController()
        {
            var rnd = new Random();

            PlainPasswords =
                new[]
                {
                    "123456", "password", "12345678", "qwerty", "123456789", "baseball", "dragon", "football", "monkey",
                    "letmein", "abc123", "111111", "mustang", "access", "shadow", "master", "michael", "superman",
                    "696969", "123123", "batman", "trustno1"
                }.Select(p => new { p, r = rnd.Next() })
                    .OrderBy(x => x.r)
                    .Select(x => x.p)
                    .ToArray();
        }

        private readonly BuggyContext _db;
        private readonly BuggyUserManager _userManager;

        public AdminController(BuggyContext db, BuggyUserManager userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("users")]
        public async Task<UserItem[]> GetUsers()
        {
            return (await _db.Users.OrderBy(x => x.UserName).ToArrayAsync()).Select(
                (x, i) => new UserItem
                    {
                        Username = x.UserName,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        LockedOut =
                            x.LockoutEnabled
                            && x.LockoutEndDateUtc.GetValueOrDefault(DateTime.MinValue) > DateTime.UtcNow,
                        Password = PlainPasswords[i % PlainPasswords.Length]
                    }).ToArray();
        }

        [HttpPut]
        [Route("users/{username}/password")]
        public async Task SetPassword(string username, [FromBody] string password)
        {
            var user = await GetUserByName(username);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await _userManager.ResetPasswordAsync(user.Id, token, password);
            if (!result.Succeeded)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", result.Errors)));
            }
        }

        [HttpPut]
        [Route("users/{username}/lock")]
        public async Task LockUser(string username)
        {
            var user = await GetUserByName(username);

            await _userManager.SetLockoutEnabledAsync(user.Id, true);
            var result = await _userManager.SetLockoutEndDateAsync(user.Id, DateTimeOffset.MaxValue);
            if (!result.Succeeded)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", result.Errors)));
            }
        }

        [HttpPut]
        [Route("users/{username}/unlock")]
        public async Task UnlockUser(string username)
        {
            var user = await GetUserByName(username);

            var result = await _userManager.SetLockoutEndDateAsync(user.Id, DateTimeOffset.UtcNow);
            if (!result.Succeeded)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(", ", result.Errors)));
            }
        }

        [HttpPost]
        [Route("reset/cars")]
        public async Task ResetCars()
        {
            var helper = new BuggyInitializerHelper(_db);
            await helper.ResetCars();
        }

        private async Task<User> GetUserByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to find the user."));
            }
            return user;
        }
    }
}