using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Services.TokenGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private AccessTokenGenerator _accessTokenGenerator;
        private RefreshTokenGenerator _refreshTokenGenerator;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public Task<AuthenticatedUserResult> Authenticate(ApplicationUser user)
        {
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.Create(refreshTokenDTO);

            return Task.FromResult(AuthenticatedUserResult.Success(accessToken, refreshToken));
        }
    }
}
