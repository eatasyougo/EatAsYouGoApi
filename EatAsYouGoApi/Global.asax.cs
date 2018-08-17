using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using EatAsYouGoApi.ExceptionHandling;

namespace EatAsYouGoApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
