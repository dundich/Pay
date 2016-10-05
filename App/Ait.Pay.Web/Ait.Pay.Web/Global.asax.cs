using Ait.Pay.Web.Providers;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ait.Pay.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.Formatters.Remove(config.Formatters.JsonFormatter); // RemoveAt(0);

            //Newtonsoft.Json.JsonSerializer.
            config.Formatters.Insert(0, new JsonNetFormatter());// new JsonMediaTypeFormatter() { });//  JsonNetFormatter());// JsonNetMediaTypeFormatter());



            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
