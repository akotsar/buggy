using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using Buggy.Data;
using Buggy.Data.Identity;

using Owin;

namespace Buggy.App_Start
{
    public class AutofacConfig
    {
        public static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<BuggyContext>().InstancePerRequest();
            builder.Register(ctx => new BuggyUserManager(ctx.Resolve<BuggyContext>()));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            app.UseAutofacMiddleware(container);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}