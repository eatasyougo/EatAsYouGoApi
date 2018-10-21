using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;
using EatAsYouGoApi.Services.Interfaces;
using log4net;
    
namespace EatAsYouGoApi.Controllers
{
    public abstract class BaseController : ApiController
    {
        //private static ILog _logger;

        protected BaseController(ILogService logService)
        {
            LogService = logService;
        }

        //protected static ILog Logger => _logger ?? (_logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        protected ILogService LogService { get; }

        protected virtual IHttpActionResult CreateResponse<T>(T value)
        {
            var response = value == null
                ? Request.CreateResponse(HttpStatusCode.NotFound)
                : Request.CreateResponse(HttpStatusCode.OK, value);

            var result = ResponseMessage(response);
            return result;
        }

        protected virtual IHttpActionResult CreateEmptyResponse()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var result = ResponseMessage(response);
            return result;
        }

        protected virtual IHttpActionResult CreateErrorResponse(string errorMessage = null, Exception exception = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            var errorResponse = exception != null
                ? Request.CreateErrorResponse(httpStatusCode, errorMessage ?? exception.Message, exception)
                : Request.CreateErrorResponse(httpStatusCode, errorMessage);

            var result = ResponseMessage(errorResponse);
            return result;
        }

        protected virtual void LogError(Type type = null, string errorMessage = null, [CallerMemberName]string methodName = null)
        {
            LogService.Error($"Error occured: {type?.Name} - {methodName} - {errorMessage}");
            //Logger.Error($"Error occured: {type?.Name} - {methodName} - {errorMessage}");
        }
    }
}