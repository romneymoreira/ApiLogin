using System.Web.Http;
using log4net;
using PDev.Auth.Api.Attributes;

namespace PDev.Auth.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           config.Filters.Add(new ErrorHandlerAttribute(LogManager.GetLogger(typeof(ErrorHandlerAttribute).FullName)));
        }
    }
}
