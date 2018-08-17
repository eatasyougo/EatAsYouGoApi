using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;
using log4net;
    
namespace EatAsYouGoApi.Controllers
{
    public abstract class BaseController : ApiController
    {
        private static ILog _logger;

        protected static ILog Logger => _logger ?? (_logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));

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
            var errorResponse = Request.CreateErrorResponse(httpStatusCode, errorMessage??exception?.Message, exception);
            var result = ResponseMessage(errorResponse);
            return result;
        }

        protected virtual void LogError(Type type = null, string errorMessage = null, [CallerMemberName]string methodName = null)
        {
            Logger.Error($"Error occured: {type?.Name} - {methodName} - {errorMessage}");
        }
    }
}