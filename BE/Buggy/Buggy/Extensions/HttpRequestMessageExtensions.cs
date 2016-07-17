using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace Buggy.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetUserId(this HttpRequestMessage request)
        {
            var claims = (request.GetRequestContext().Principal as ClaimsPrincipal)?.Claims;

            return claims?.SingleOrDefault(x => x.Type == "id")?.Value;
        }
    }
}