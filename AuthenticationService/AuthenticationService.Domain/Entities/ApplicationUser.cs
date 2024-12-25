using AuthenticationService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public Username Username { get; } = null!;
        public PasswordHash HashPassword { get; } = null!;
        public IReadOnlyCollection<RefreshToken> RefreshTokens = new List<RefreshToken>();

        public ApplicationUser() { }
        public ApplicationUser(Username username, PasswordHash hashPassword)
        {
            Id = Guid.NewGuid();
            Username = username;
            HashPassword = hashPassword;
        }
    }
}
