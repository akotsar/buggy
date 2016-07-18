using System.Web;
using System.Web.Http.Filters;

using Elmah;

namespace Buggy.Filters
{
    public class ElmahExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ErrorLog.GetDefault(HttpContext.Current).Log(new Error(actionExecutedContext.Exception));
        }
    }
}