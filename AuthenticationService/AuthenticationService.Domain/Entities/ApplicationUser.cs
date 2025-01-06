using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public Username Username { get; } = null!;
        public PasswordHash HashPassword { get; } = null!;
        public RoleEnum Role {  get; set; } 
        public IReadOnlyCollection<RefreshToken> RefreshTokens = new List<RefreshToken>();

        public ApplicationUser() { }
        public ApplicationUser(Username username, PasswordHash hashPassword, RoleEnum role)
        {
            Id = Guid.NewGuid();
            Username = username;
            HashPassword = hashPassword;
            Role = role;
        }
    }
}
