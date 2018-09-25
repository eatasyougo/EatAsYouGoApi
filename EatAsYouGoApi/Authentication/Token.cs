using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Helper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EatAsYouGoApi.Authentication
{
    public class Token
    {
        private const string WebUrl = "http://eatasyougoapi.azurewebsites.net";

        public static string CreateNewToken(UserDto userDto, int? tokenExpiryInMins)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, $"{userDto.FirstName} {userDto.LastName}"),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim("UserId", userDto.UserId.ToString()),
                new Claim("CurrentUserGroups", JsonConvert.SerializeObject(userDto.Groups)),
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(tokenExpiryInMins ?? 10);
            var tokenHandler = new JwtSecurityTokenHandler();
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

        public static string CreateNewToken(ClaimsPrincipal claimsPrincipal, int? tokenExpiryInMins)
        {
            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(tokenExpiryInMins ?? 10);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(
                WebUrl,
                WebUrl,
                claimsPrincipal.Identity as ClaimsIdentity,
                issuedAt,
                expires,
                null,
                signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }
    }
}