using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using EatAsYouGoApi.Dtos;
using Newtonsoft.Json;

namespace EatAsYouGoApi.Authentication
{
    public class AuthorizeGroupsAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var groups = Groups?.Split(',').Where(g => g != null).Select(g => g.Trim());
            if (groups == null)
            {
                base.OnAuthorization(actionContext);
                return;
            }
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (identity == null)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User unauthorized");
                return;
            }

            var currentUserObject = identity.Claims.FirstOrDefault(c => c.Type.Equals("CurrentUser"))?.Value;
            if(currentUserObject == null)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User unauthorized");
                return;
            }

            var currentUser = JsonConvert.DeserializeObject<UserDto>(
                currentUserObject,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Include});

            var authorized = groups.Any(g => currentUser.Groups.Any(group => @group.GroupName.ToLower().Equals(g.ToLower())));
            if (!authorized)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User unauthorized");
                return;
            }

            base.OnAuthorization(actionContext);
        }
    }
}