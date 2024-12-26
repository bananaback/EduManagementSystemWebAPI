using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Common.Interfaces.TokenValidators;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Login;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.RotateToken
{
    public class RotateTokenCommandHandler : IRequestHandler<RotateTokenCommand, AuthenticatedUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenValidator _tokenValidator;
        private readonly IAuthenticator _authenticator;
        
        public RotateTokenCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository, 
            IUnitOfWork unitOfWork,
            ITokenValidator tokenValidator,
            IAuthenticator authenticator)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenValidator = tokenValidator;
            _authenticator = authenticator;
        }
        public async Task<AuthenticatedUserResult> Handle(RotateTokenCommand command, CancellationToken cancellationToken)
        {
            bool isValidRefreshToken = _tokenValidator.Validate(command.RefreshToken);

            if (!isValidRefreshToken)
            {
                return AuthenticatedUserResult.Failure();
            }

            // Check if token revoked or not
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                return AuthenticatedUserResult.Failure();
            }

            _refreshTokenRepository.Delete(refreshToken);

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if (user == null)
            {
                throw new UserRetrievalException($"User with id {refreshToken.UserId} not found while trying to rotate refresh token.");
            }

            // this function already call unit of work for us
            return await _authenticator.Authenticate(user, cancellationToken);
        }
    }
}
