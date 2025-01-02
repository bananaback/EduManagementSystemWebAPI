using AuthenticationService.Application.Common.Interfaces.TokenGenerators;
using AuthenticationService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AccessTokenGenerator()
        {

        }

        public AccessTokenGenerator(AuthenticationConfiguration authenticationConfiguration, ITokenGenerator tokenGenerator)
        {
            _configuration = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;
        }

        public virtual string GenerateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.Value),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
            _configuration.Issuer,
                _configuration.Audiences,
                _configuration.AccessTokenExpirationMinutes,
                claims);
        }
    }
}
