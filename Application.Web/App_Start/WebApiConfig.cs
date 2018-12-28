using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Application.Web.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: Add any additional configuration code.

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Controllers with Actions
            // To handle routes like `/api/Account/route`
            config.Routes.MapHttpRoute(
                "ControllerAndAction",
                "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});

            // WebAPI when dealing with JSON & JavaScript!
            // Setup json serialization to serialize classes to camel (std. Json format)
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Enforce HTTPS
            config.Filters.Add(new Filters.RequireHttpsAttribute());
        }
    }
}