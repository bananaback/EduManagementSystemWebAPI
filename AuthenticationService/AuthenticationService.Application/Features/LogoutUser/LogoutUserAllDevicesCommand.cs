using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.LogoutUser
{
    public class LogoutUserAllDevicesCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
