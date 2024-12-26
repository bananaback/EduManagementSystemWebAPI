using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticatedUserResult> Authenticate(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshTokenString = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshToken = new RefreshToken()
            {
                User = user,
                Token = TokenValue.Create(refreshTokenString)
            };

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new TokenPersistenceException("Failed to save refres token while authenticate user.");
            }

            return AuthenticatedUserResult.Success(accessToken, refreshTokenString);
        }
    }
}
