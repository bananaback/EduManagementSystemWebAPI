using AuthenticationService.Application.Common.Interfaces.TokenValidators;
using AuthenticationService.Infrastructure.Services.TokenGenerators;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenValidators
{
    public class RefreshTokenValidator : ITokenValidator
    {
        private readonly AuthenticationConfiguration _configuration;

        public RefreshTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            _configuration = authenticationConfiguration;
        }

        public bool Validate(string refreshToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret)),
                ValidIssuer = _configuration.Issuer,
                ValidAudiences = _configuration.Audiences,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                handler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
