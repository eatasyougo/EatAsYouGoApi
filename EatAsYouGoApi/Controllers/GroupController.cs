using System;
using System.Web.Http;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    [AuthorizeGroups(Groups = "SiteAdministrators")]
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService, ILogService logService)
            : base(logService)
        {
            _groupService = groupService;
        }

        [SwaggerDescription("Gets all groups. Shows only active groups if showActiveGroupsOnly is set to true.", "Gets all groups")]
        [Route("api/groups/getAll/{showActiveGroupsOnly}")]
        public IHttpActionResult GetAllGroups(bool showActiveGroupsOnly)
        {
            try
            {
                var groups = _groupService.GetAllGroups(showActiveGroupsOnly);
                return CreateResponse(groups);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets group by id", "Gets group by id")]
        [Route("api/groups/get/{groupId}")]
        public IHttpActionResult GetGroupById(int groupId)
        {
            try
            {
                var group = _groupService.GetGroupById(groupId);
                return CreateResponse(group);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Add new group", "Adds new group")]
        [Route("api/groups/add")]
        public IHttpActionResult AddNewGroup(GroupDto groupDto)
        {
            try
            {
                if (groupDto == null)
                    return CreateErrorResponse($"Parameter {nameof(groupDto)} cannot be null");

                var group = _groupService.AddNewGroup(groupDto);
                return CreateResponse(group);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }


        [SwaggerDescription("Removes a group", "Removes a group")]
        [Route("api/groups/delete/{groupId}")]
        [HttpPost]
        public IHttpActionResult RemoveGroup(int groupId)
        {
            try
            {
                if (groupId == 0)
                    return CreateErrorResponse($"Parameter {nameof(groupId)} must be greater than 0");

                _groupService.RemoveGroup(groupId);
                return CreateEmptyResponse();
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Updates a group", "Updates a group")]
        [Route("api/groups/update")]
        [HttpPost]
        public IHttpActionResult UpdateGroup(GroupDto groupDto)
        {
            try
            {
                if (groupDto == null)
                    return CreateErrorResponse($"Parameter {nameof(groupDto)} cannot be null");

                var updatedRestaurantDto = _groupService.UpdateGroup(groupDto);
                return CreateResponse(updatedRestaurantDto);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }
    }
}
