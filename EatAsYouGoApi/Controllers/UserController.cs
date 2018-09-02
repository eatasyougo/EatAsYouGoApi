using System;
using System.Net;
using System.Web.Http;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
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

                if (user == null)
                    CreateErrorResponse($"No user found with email - {email}");

                return CreateResponse(user);
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
                    CreateErrorResponse($"Parameter {nameof(userDto)} cannot be null");

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
                    CreateErrorResponse($"Parameter {nameof(userId)} must be greater than 0");

                _userService.RemoveUser(userId);
                return CreateEmptyResponse();
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
        public IHttpActionResult UpdateRestaurant(UserDto userDto)
        {
            try
            {
                if (userDto == null)
                    CreateErrorResponse($"Parameter {nameof(userDto)} cannot be null");

                var updatedRestaurantDto = _userService.UpdateUser(userDto);
                return CreateResponse(updatedRestaurantDto);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        //[SwaggerDescription("Validate user", "Validate user")]
        //[Route("api/users/validate")]
        //[HttpPost]
        //public IHttpActionResult ValidateUser(UserDto userDto)
        //{
        //    try
        //    {
        //        var validated = _userService.ValidateUser(userDto);

        //        if (validated)
        //            CreateErrorResponse("Failed to validate user");

        //        return CreateResponse(userDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        LogError(this.GetType(), exception.Message);
        //        return CreateErrorResponse(exception.Message, exception);
        //    }
        //}
    }
}
