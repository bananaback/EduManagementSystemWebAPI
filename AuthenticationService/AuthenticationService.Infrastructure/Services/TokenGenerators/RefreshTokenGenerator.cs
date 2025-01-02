using AuthenticationService.Application.Common.Interfaces.TokenGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenGenerator()
        {

        }

        public RefreshTokenGenerator(AuthenticationConfiguration authenticationConfiguration, ITokenGenerator tokenGenerator)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;
        }

        public virtual string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(
                _authenticationConfiguration.RefreshTokenSecret,
                _authenticationConfiguration.Issuer,
                _authenticationConfiguration.Audiences,
                _authenticationConfiguration.RefreshTokenExpirationMinutes,
                new List<Claim>());
        }
    }
}
