using System;
using System.Reflection;
using EatAsYouGoApi.Services.Interfaces;
using log4net;
using log4net.Core;

namespace EatAsYouGoApi.Services
{
    public class LogService : ILogService
    {
        private static readonly ILog Log = LogManager.GetLogger(Assembly.GetCallingAssembly().GetType());

        public void Error(string errorMessage, Exception exception = null)
        {
            if (exception == null)
                Log.Error(errorMessage);
            else
                Log.Error(errorMessage, exception);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }
    }
}