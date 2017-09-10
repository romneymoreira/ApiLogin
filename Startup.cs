using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using PDev.Auth.Api.Service;
using PDev.Auth.Api.Service.Interface;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using Microsoft.Owin.Security.OAuth;
using PDev.Auth.Api.Providers;
using Container = SimpleInjector.Container;

[assembly: OwinStartup(typeof(PDev.Auth.Api.Startup))]

namespace PDev.Auth.Api 
{
    public class Startup
    {
        public Container Container = null;
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            InitializeContainer(container);
            container.Verify();

            app.Use(async (context, next) =>
            {
                using (container.BeginExecutionContextScope())
                {
                    await next();
                }
            });

            var config = new HttpConfiguration();

            ConfigureOAuth(app);

            //config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this.Container);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.UseWebApi(config);
        }

        private static void InitializeContainer(Container container)
        {
            //container.Register<Security>(Lifestyle.Scoped);
           // container.Register(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifestyle.Scoped);
            container.RegisterWebApiRequest<IUserService, UserService>();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApiAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
