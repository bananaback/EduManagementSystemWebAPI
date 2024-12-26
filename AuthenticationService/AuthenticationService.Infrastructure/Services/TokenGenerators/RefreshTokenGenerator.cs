using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration authenticationConfiguration, TokenGenerator tokenGenerator)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(
                _authenticationConfiguration.RefreshTokenSecret,
                _authenticationConfiguration.Issuer,
                _authenticationConfiguration.Audiences,
                _authenticationConfiguration.RefreshTokenExpirationMinutes);
        }
    }
}
