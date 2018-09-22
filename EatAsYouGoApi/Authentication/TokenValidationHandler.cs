using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace EatAsYouGoApi.Authentication
{
    public class TokenValidationHandler : DelegatingHandler
    {
        private const string WebUrl = "http://eatasyougoapi.azurewebsites.net";
        private string _expiryMessage = string.Empty;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token;
            //determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                //allow requests with no token - whether a action method needs an authentication can be set with the claims authorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            HttpResponseMessage responseMessage;
            try
            {
                const string secKeyToken = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secKeyToken));
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = WebUrl,
                    ValidIssuer = WebUrl,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey 
                };
                
                SecurityToken securityToken;
                var handler = new JwtSecurityTokenHandler();
                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);
                
                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                responseMessage = request.CreateErrorResponse(HttpStatusCode.Unauthorized, $"Token validation error: {_expiryMessage}", e);
            }
            catch (Exception ex)
            {
                responseMessage = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => responseMessage, cancellationToken);
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            _expiryMessage = string.Empty;
            if (expires == null)
                return false;

            var valid = DateTime.UtcNow < expires;
            if (!valid)
                _expiryMessage = "Token expired!";

            return valid;
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authorizationHeaders;
            request.Headers.TryGetValues("Authorization", out authorizationHeaders);

            var authHeaders = authorizationHeaders?.ToList();
            if ( authHeaders == null || authHeaders.Count > 1)
                return false;

            var bearerToken = authHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }
}