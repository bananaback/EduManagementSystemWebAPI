using AuthenticationService.Application.Features.Login;
using AuthenticationService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.RotateToken
{
    public class RotateTokenCommand : IRequest<AuthenticatedUserResult>
    {
        public TokenValue RefreshToken { get; private set; } = null!;
        public RotateTokenCommand(string refreshToken)
        {
            RefreshToken = TokenValue.Create(refreshToken);
        }
    }
}
