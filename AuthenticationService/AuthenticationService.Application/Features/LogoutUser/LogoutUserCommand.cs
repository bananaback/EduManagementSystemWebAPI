using AuthenticationService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.LogoutUser
{
    public class LogoutUserCommand : IRequest<bool>
    {
        public TokenValue RefreshToken { get; private set; } = null!;
        public LogoutUserCommand(string refreshToken)
        {
            RefreshToken = TokenValue.Create(refreshToken);
        }
    }
}
