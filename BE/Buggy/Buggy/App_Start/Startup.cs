using System;
using System.Web.Cors;
using System.Web.Http;

using Buggy.App_Start;
using Buggy.Auth;

using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;

using Owin;

[assembly: OwinStartup(typeof(Buggy.Startup))]
namespace Buggy
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            AutofacConfig.Configure(app, config);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
                                                                 {
                                                                     AllowInsecureHttp = true,
                                                                     TokenEndpointPath = new PathString("/oauth/token"),
                                                                     AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                                                                     Provider = new BuggyAuthorizationServerProvider()
                                                                 };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
