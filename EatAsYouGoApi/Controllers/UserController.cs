using System;
using System.Web.Http;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    [AuthorizeGroups(Groups = "SiteAdministrators")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogService logService)
            : base(logService)
        {
            _userService = userService;
        }

        [SwaggerDescription("Gets all users. Shows only active users if showActiveUsersOnly is set to true.", "Gets all users")]
        [Route("api/users/getall/{showActiveUsersOnly}")]
        public IHttpActionResult GetAllUsers(bool showActiveUsersOnly)
        {
            try
            {
                var users = _userService.GetAllUsers(showActiveUsersOnly);
                return CreateResponse(users);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets user by id", "Gets user by id")]
        [Route("api/users/get/{userId}")]
        public IHttpActionResult GetUserById(long userId)
        {
            try
            {
                var user = _userService.GetUserById(userId);
                return CreateResponse(user);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets user by email", "Gets user by email")]
        [Route("api/users/get/email/{email}")]
        public IHttpActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.GetUserByEmail(email);

                return user == null 
                    ? CreateErrorResponse($"No user found with email - {email}") 
                    : CreateResponse(user);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Add new user", "Adds new user")]
        [Route("api/users/add")]
        [HttpPost]
        public IHttpActionResult AddNewUser(UserDto userDto)
        {
            try
            {
                if (userDto == null)
                    return CreateErrorResponse($"Parameter {nameof(userDto)} cannot be null");

                var user = _userService.AddNewUser(userDto);
                return CreateResponse(user);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }


        [SwaggerDescription("Removes a user", "Removes a user")]
        [Route("api/users/delete/{userId}")]
        [HttpPost]
        public IHttpActionResult RemoveUser(long userId)
        {
            try
            {
                if (userId == 0)
                    return CreateErrorResponse($"Parameter {nameof(userId)} must be greater than 0");

                _userService.RemoveUser(userId);
                return CreateResponse("User successfully removed.");
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Updates a user", "Updates a user")]
        [Route("api/users/update")]
        [HttpPost]
        public IHttpActionResult UpdateUser(UserDto userDto)
        {
            try
            {
                if (userDto == null)
                    return CreateErrorResponse($"Parameter {nameof(userDto)} cannot be null");

                var updatedUserDto = _userService.UpdateUser(userDto);
                return CreateResponse(updatedUserDto);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }
    }
}
