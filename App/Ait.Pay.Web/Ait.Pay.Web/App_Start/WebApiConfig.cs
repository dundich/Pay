﻿using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace Ait.Pay.Web
{
    public static class WebApiConfig
    {
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //I chaned the routeTemplate so that methods/services would be identified by their action, and not by their parameters.
            //I was getting conflicts if I had more than one GET services, that had identical parameter options, but totally different return data.
            //Adding the action to the routeTemplte correct this issue.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}", //routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //app.UseWebApi(config);
        }
    }
}
