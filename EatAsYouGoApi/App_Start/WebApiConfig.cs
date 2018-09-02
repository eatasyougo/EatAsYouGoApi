using System.Net.Http.Formatting;
using System.Web.Http;
using AutoMapper;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Core;
using EatAsYouGoApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Unity;

namespace EatAsYouGoApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityRegistrations.Register(config);
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfile()));

            // Add filters
            //var unityDependencyResolver = config.DependencyResolver as UnityDependencyResolver;
            //var userService = unityDependencyResolver?.Container.Resolve<IUserService>();
            //config.Filters.Add(new JsonTokenAuthenticationFilter(userService));

            config.MessageHandlers.Add(new TokenValidationHandler());

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
