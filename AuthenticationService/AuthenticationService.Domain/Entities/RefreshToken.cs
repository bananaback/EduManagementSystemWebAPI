using AuthenticationService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public TokenValue Token { get; set; } = null!;

        public RefreshToken() { }
        public RefreshToken(ApplicationUser user, TokenValue token)
        {
            Id = Guid.NewGuid();
            User = user;
            UserId = user.Id;
            Token = token;
        }
    }
}
