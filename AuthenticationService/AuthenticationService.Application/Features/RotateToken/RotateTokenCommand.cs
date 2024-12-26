using AuthenticationService.Application.Features.Login;
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
        public string RefreshToken { get; set; } = string.Empty;
    }
}
