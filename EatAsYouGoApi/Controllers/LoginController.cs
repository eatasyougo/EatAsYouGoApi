using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Helper;
using EatAsYouGoApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;

        public LoginController(IUserService userService, ISecurityService securityService, ILogService logService)
            : base(logService)
        {
            _userService = userService;
            _securityService = securityService;
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

                var password = _securityService.EncryptToString(loginRequestDto.Password);
                var user = _userService.GetUserByEmailAndPassword(loginRequestDto.Email, password, true);

                if (user == null)
                    return CreateErrorResponse("Invalid login! Login details not found");

                var authToken = Token.CreateNewToken(user, Config.AuthTokenExpiryInMins);
                var refreshToken = Token.CreateNewToken(user, Config.RefreshTokenExpiryInMins);

                user.RefreshToken = refreshToken;
                var updatedUser = _userService.UpdateUser(user);

                var userToken = new UserToken
                {
                    Authorization = authToken,
                    Refresh = refreshToken,
                    User = updatedUser
                };

                return CreateResponse(userToken);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Register a new user", "Registers a new user")]
        [Route("api/users/register")]
        [HttpPost]
        public IHttpActionResult AddNewUser(UserDto userDto)
        {
            try
            {
                if (userDto == null)
                    return CreateErrorResponse($"Parameter {nameof(userDto)} cannot be null");

                var password = _securityService.EncryptToString(userDto.Password);
                userDto.Password = password;

                var user = _userService.AddNewUser(userDto);
                return CreateResponse(user);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Refreshes an authentication token", "Refreshes an authentication token if expired by providing a new one")]
        [Route("api/token/refresh")]
        [HttpPost]
        public IHttpActionResult RefreshToken()
        {
            string refreshToken;
            TokenValidationHandler.TryRetrieveRefreshToken(Request, out refreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
                return CreateErrorResponse("User unauthorized as no valid token found.");

            try
            {
                var claimsPrincipal = TokenValidationHandler.GetPrincipal(refreshToken, LifetimeValidator) as ClaimsPrincipal;
                if (claimsPrincipal == null)
                    throw new InvalidOperationException("Token vannot be validated. Failed to get User Principal");

                var emailClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Email));
                var email = emailClaim?.Value;
                if (string.IsNullOrWhiteSpace(email))
                    return CreateErrorResponse("User unauthorized as no valid refresh token found.");

                var user = _userService.GetUserByEmail(email);
                if(!user.RefreshToken.Equals(refreshToken))
                    return CreateErrorResponse("User unauthorized as no valid refresh token found.");

                var newToken = Token.CreateNewToken(claimsPrincipal, Config.AuthTokenExpiryInMins);

                return CreateResponse(new UserToken { Authorization = newToken, Refresh = refreshToken });
            }
            catch (SecurityTokenExpiredException tokenExpiredException)
            {
                return CreateErrorResponse(
                    $"Refresh token validation error: {tokenExpiredException}",
                    tokenExpiredException,
                    HttpStatusCode.Unauthorized);
            }
            catch (SecurityTokenValidationException tokenValidationException)
            {
                return CreateErrorResponse(
                    $"Refresh token validation error: {tokenValidationException.Message}",
                    tokenValidationException,
                    HttpStatusCode.Unauthorized);
            }
            catch (Exception exception)
            {
                return CreateErrorResponse(
                    exception.Message,
                    exception, 
                    HttpStatusCode.InternalServerError);
            }
        }


        private bool LifetimeValidator(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            if (expires == null)
                return false;

            if (DateTime.UtcNow > expires)
                throw new SecurityTokenExpiredException("Refresh token expired!");

            return true;
        }
    }

    public class UserToken
    {
        public string Authorization { get; set; }

        public string Refresh { get; set; }

        public UserDto User { get; set; }
    }
}
