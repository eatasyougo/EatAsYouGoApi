using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using EatAsYouGoApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        private const string WebUrl = "http://eatasyougoapi.azurewebsites.net";

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerDescription("Login", "Submits a Login request")]
        [Route("api/users/login")]
        [HttpPost]
        public IHttpActionResult LoginUser(LoginRequest loginRequest)
        {
            try
            {
                if(loginRequest == null)
                    return CreateErrorResponse("Please enter Email and Password to continue.");

                if (string.IsNullOrWhiteSpace(loginRequest.Email))
                    return CreateErrorResponse($"{nameof(loginRequest.Email)} cannot be empty");

                if (string.IsNullOrWhiteSpace(loginRequest.Password))
                    return CreateErrorResponse($"{nameof(loginRequest.Password)} cannot be empty");

                var user = _userService.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

                if (user == null)
                    return CreateErrorResponse("Invalid login! Login details not found");

                var authToken = CreateToken(user.Email);

                return CreateResponse(authToken);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        private static string CreateToken(string email)
        {
            //Set issued at date
            var issuedAt = DateTime.UtcNow;
            //set the time when it expires
            var expires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token = tokenHandler.CreateJwtSecurityToken(
                WebUrl, 
                WebUrl,
                claimsIdentity, 
                issuedAt, 
                expires, 
                null,
                signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }

    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [PasswordPropertyText]
        public string  Password { get; set; }
    }
}
