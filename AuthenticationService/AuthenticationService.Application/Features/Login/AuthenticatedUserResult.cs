using AuthenticationService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Login
{
    public class AuthenticatedUserResult
    {
        public bool IsSuccess { get; set; }
        public TokenValue? AccessToken { get; set; } 
        public TokenValue? RefreshToken { get; set; }

        private AuthenticatedUserResult(bool isSuccess, string accessToken, string refreshToken)
        {
            IsSuccess = isSuccess;
            AccessToken = TokenValue.Create(accessToken);
            RefreshToken = TokenValue.Create(refreshToken);
        }

        private AuthenticatedUserResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        
        public static AuthenticatedUserResult Success(string accessToken, string refreshToken)
        {
            return new AuthenticatedUserResult(true, accessToken, refreshToken);
        }

        public static AuthenticatedUserResult Failure()
        {
            return new AuthenticatedUserResult(false);
        }
    }
}
