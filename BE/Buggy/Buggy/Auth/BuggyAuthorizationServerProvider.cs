using System.Security.Claims;
using System.Threading.Tasks;

using Buggy.Data;
using Buggy.Data.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;

namespace Buggy.Auth
{
    public class BuggyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new BuggyContext())
            {
                var userManager = new BuggyUserManager(db);
                IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    context.SetError("invalid_grant", "The user is locked out.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("id", user.Id));
                identity.AddClaim(new Claim("roles", string.Join(",", await userManager.GetRolesAsync(user.Id))));

                context.Validated(identity);
            }
        }
    }
}