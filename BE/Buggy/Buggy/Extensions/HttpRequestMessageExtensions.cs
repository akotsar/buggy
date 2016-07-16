using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Buggy.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetUserId(this HttpRequestMessage request)
        {
            var claims = (request.GetRequestContext().Principal as ClaimsPrincipal)?.Claims;
            if (claims == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            return claims.Single(x => x.Type == "id").Value;
        }
    }
}