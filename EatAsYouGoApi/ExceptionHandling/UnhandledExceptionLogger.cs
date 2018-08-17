using System.Reflection;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace EatAsYouGoApi.ExceptionHandling
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void Log(ExceptionLoggerContext context)
        {
            var log = context.Exception.ToString();

            //Write the exception to your logs
            Logger.Fatal(log, context.Exception);
        }
    }
}