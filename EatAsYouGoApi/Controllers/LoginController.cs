using System;
using System.Web;
using System.Web.Http;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerDescription("Login", "Submits a Login request")]
        [Route("api/users/login")]
        [HttpPost]
        public IHttpActionResult LoginUser(LoginRequestDto loginRequestDto)
        {
            try
            {
                if(loginRequestDto == null)
                    return CreateErrorResponse("Please enter Email and Password to continue.");

                if (string.IsNullOrWhiteSpace(loginRequestDto.Email))
                    return CreateErrorResponse($"{nameof(loginRequestDto.Email)} cannot be empty");

                if (string.IsNullOrWhiteSpace(loginRequestDto.Password))
                    return CreateErrorResponse($"{nameof(loginRequestDto.Password)} cannot be empty");

                var user = _userService.GetUserByEmailAndPassword(loginRequestDto.Email, loginRequestDto.Password, true);

                if (user == null)
                    return CreateErrorResponse("Invalid login! Login details not found");

                var authToken = Authentication.Authentication.CreateToken(user);

                return CreateResponse(authToken);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }
    }
}
