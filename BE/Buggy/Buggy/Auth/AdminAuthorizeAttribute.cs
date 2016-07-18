using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Buggy.Auth
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var result = base.IsAuthorized(actionContext);
            if (!result)
            {
                return false;
            }

            var claimsPrincipal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }

            var roles = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "roles")?.Value ?? string.Empty;
            return roles.Split(',').Contains("admin");
        }
    }
}