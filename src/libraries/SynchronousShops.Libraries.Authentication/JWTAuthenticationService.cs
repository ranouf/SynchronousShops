using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SynchronousShops.Libraries.Authentication.Configuration;
using SynchronousShops.Libraries.Authentication.Helper;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SynchronousShops.Libraries.Authentication
{
    public class JWTAuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public JWTAuthenticationService(IOptions<AuthenticationSettings> authenticationSettings)
        {
            _authenticationSettings = authenticationSettings.Value;
        }

        public string GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _authenticationSettings.Issuer,
                Audience = _authenticationSettings.Audience,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.ExpirationDurationInDays),
                SigningCredentials = JWTHelper.GetSigningCredentials(_authenticationSettings.SecretKey)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenId = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenId);
        }
    }
}
