using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.LogoutUser
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, bool>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutUserCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                return false;
            }

            _refreshTokenRepository.Delete(refreshToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
