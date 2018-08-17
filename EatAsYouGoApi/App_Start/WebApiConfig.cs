using System.Net.Http.Formatting;
using System.Web.Http;
using AutoMapper;
using EatAsYouGoApi.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EatAsYouGoApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityRegistrations.Register(config);
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfile()));

            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                Indent = true,
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            config.Formatters.Add(jsonFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"^[0-9]*$" }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
