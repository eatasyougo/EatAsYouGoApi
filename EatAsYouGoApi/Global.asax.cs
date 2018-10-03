using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.ExceptionHandling;
using Unity;

namespace EatAsYouGoApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static UnityContainer Container { get; set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
