using System.Collections.Generic;
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
            var allowedGroups = Groups?.Split(',').Where(g => g != null).Select(g => g.Trim());
            if (allowedGroups == null)
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

            var currentUserGroups = identity.Claims.FirstOrDefault(c => c.Type.Equals("CurrentUserGroups"))?.Value;
            if(currentUserGroups == null)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User unauthorized");
                return;
            }

            var permittedGroups = JsonConvert.DeserializeObject<ICollection<GroupDto>>(
                currentUserGroups,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Include});

            var authorized = allowedGroups.Any(g => permittedGroups.Any(group => group.GroupName.ToLower().Equals(g.ToLower())));
            if (!authorized)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User unauthorized");
                return;
            }

            base.OnAuthorization(actionContext);
        }
    }
}