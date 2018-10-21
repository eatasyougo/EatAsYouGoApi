using System;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface ILogService
    {
        void Error(string errorMessage, Exception exception = null);

        void Info(string message);
    }
}