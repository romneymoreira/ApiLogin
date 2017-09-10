using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PDev.Auth.Api.Domain;
using PDev.Auth.Api.Models;
using PDev.Auth.Api.Service;

namespace PDev.Auth.Api.Providers
{
    public class ApiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                var data = await context.Request.ReadFormAsync();
                string module = data.Get("module");
                User user;
                LoginResultModel result;

                using (var service = new AuthorizationService())
                {
                    result = await service.LogInAsync(context.UserName, context.Password, module);

                    if (result.Success)
                    {
                        user = result.User;
                        string roles = string.Empty;
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("user", context.UserName));
                        identity.AddClaim(new Claim("id_user", user.Id.ToString()));
                        identity.AddClaim(new Claim("name", user.Name));

                        if (user.Roles != null)
                        {
                            roles = Newtonsoft.Json.JsonConvert.SerializeObject(user.Roles.Select(r => r.Identifier));
                            identity.AddClaim(new Claim("roles", roles));
                        }

                        var props = new AuthenticationProperties(new Dictionary<string, string>()
                        {
                            { "user", context.UserName },
                            { "id_user", user.Id.ToString() },
                            { "name", user.Name },
                            { "roles", roles },
                        }); 

                        var ticket = new AuthenticationTicket(identity, props);

                        //var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                        //var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                        context.Validated(ticket);
                    }
                    else
                    {
                        context.SetError("invalid_grant", result.Message);
                        return;
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            //var accessToken = context.AccessToken;
            //var expiration = DateTime.Parse(context.Properties.Dictionary[".expires"]);
            //string userName = context.Properties.Dictionary["user"];

            //var service = new AuthorizationService();
            //Task.Run(async () => await service.LogUserAccess(new UserToken(accessToken, userName, expiration)));

            //foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            //{
            //    context.AdditionalResponseParameters.Add(property.Key, property.Value);
            //}

            return Task.FromResult<object>(null);
        }



    }
}
