﻿using Ait.Auth.Api.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Ait.Auth.Api.Providers
{
    public class AuthServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;


            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");                
                return;//Task.FromResult<object>(null);
            }

            var _repo = context.OwinContext.GetAuthRep();

            Client client = await _repo.FindClientAsync(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Клиент '{0}' не зарегистрирован в системе.", context.ClientId));
                return;// Task.FromResult<object>(null);
            }

            if (client.ApplicationType == Models.ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Клиентский ключ (Secret) не определен.");
                    return;// Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Клиентский ключ (Secret), является недействительным.");
                        return;// Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Клиент отключен.");
                return;// Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.OwinContext.Set<Client>("oauth:client", client);

            context.Validated();
            return;// Task.FromResult<object>(null);
        }


        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{

        //    var allowedOrigin = "*";

        //    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

        //    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

        //    ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

        //    if (user == null)
        //    {
        //        context.SetError("invalid_grant", "The user name or password is incorrect.");
        //        return;
        //    }

        //    if (!user.EmailConfirmed)
        //    {
        //        context.SetError("invalid_grant", "User did not confirm email.");
        //        return;
        //    }

        //    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
        //    oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
        //    oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));

        //    var ticket = new AuthenticationTicket(oAuthIdentity, null);

        //    context.Validated(ticket);

        //}


        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            //Client validated, generate token
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Client client = context.OwinContext.Get<Client>("oauth:client");
            if (client != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType));
                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                // Add an custum Claim with the additional parameter
                //identity.AddClaim(new Claim("Participant", context.OwinContext.Get<Participant>("urn:participant").Document));
                context.Validated(identity);

                var props = CreateProperties(client.Name, context.ClientId);

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
            {
                context.SetError(
                    "unauthorized_client",
                    "Клиент не имеет права использовать этот тип авторизации (grant)");
            }

            return Task.FromResult(0);
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var _repo = context.OwinContext.GetAuthRep();

            IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Имя пользователя или пароль неверны!");
                return;
            }


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            var props = CreateProperties(context.UserName, context.ClientId);

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public static AuthenticationProperties CreateProperties(string userName, string ClientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "as:client_id", ClientId ?? string.Empty },
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Маркер обновления выдается другой ClientID.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}