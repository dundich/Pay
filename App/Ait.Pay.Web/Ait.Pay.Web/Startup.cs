﻿using Ait.Pay.Web.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Ait.Pay.Web.Startup))]

namespace Ait.Pay.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //AutofacConfig.ConfigureContainer(app);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
            AutofacConfig.ConfigureContainer(app);
        }
    }
}
