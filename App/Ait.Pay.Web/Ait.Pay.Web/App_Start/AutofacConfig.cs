using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace Ait.Pay.Web.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer(IAppBuilder app)
        {

            var builder = new ContainerBuilder();

            // Register our Data dependencies
            builder.RegisterModule<PayModule>();

            var a = Assembly.GetExecutingAssembly();
            // Register your Web API controllers.
            builder.RegisterApiControllers(a);
            //mvc
            builder.RegisterControllers(a);


            var container = builder.Build();

            // OWIN WEB API SETUP:
            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);

            app.UseAutofacWebApi(GlobalConfiguration.Configuration);

            app.UseAutofacMvc();


            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //var dataAccess = Assembly.GetExecutingAssembly();
            //Type lookupType = typeof(Maybe2.Classes.IDependency);
            //builder.RegisterAssemblyTypes(dataAccess)
            //       .Where(t => t.IsAssignableFrom(t) && !t.IsInterface)
            //       .AsImplementedInterfaces();

        }
    }
}