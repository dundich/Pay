using Ait.Auth.Api.Modules;
using Ait.Auth.Api.Providers;
using Maybe2;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

[assembly: OwinStartup(typeof(Ait.Auth.Api.Startup))]

namespace Ait.Auth.Api
{
    public class Startup
    {

        readonly AuthShell rootShell = new AuthShell();

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();
            config.Formatters.Remove(config.Formatters.JsonFormatter); // RemoveAt(0);
            //Newtonsoft.Json.JsonSerializer.
            config.Formatters.Insert(0, new JsonMediaTypeFormatter());

            app.Use<TenantModule>("Tenant Module >");

            app.Use<SetupModule>("Setup Module >");

            ConfigureOAuth(app);

            WebApiConfig.Register(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AuthServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            if (!rootShell.Config[AuthConsts.OAuthGoogleClientId_KEY].IsNullOrWhiteSpace())
            {
                //Configure Google External Login
                googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = rootShell.Config[AuthConsts.OAuthGoogleClientId_KEY],
                    ClientSecret = rootShell.Config[AuthConsts.OAuthGoogleClientSecret_KEY],

                    Provider = new GoogleAuthProvider()
                };
                app.UseGoogleAuthentication(googleAuthOptions);
            }

            if (!rootShell.Config[AuthConsts.OAuthFacebookAppId_KEY].IsNullOrWhiteSpace())
            {
                //Configure Facebook External Login
                facebookAuthOptions = new FacebookAuthenticationOptions()
                {
                    AppId = rootShell.Config[AuthConsts.OAuthFacebookAppId_KEY],
                    AppSecret = rootShell.Config[AuthConsts.OAuthFacebookAppSecret_KEY],
                    UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name",
                    BackchannelHttpHandler = new FacebookBackChannelHandler(),
                    Scope = { "email" },
                    Provider = new FacebookAuthProvider()
                };

                //facebookAuthOptions.Scope.Add("email");

                app.UseFacebookAuthentication(facebookAuthOptions);
            }

        }
    }

}