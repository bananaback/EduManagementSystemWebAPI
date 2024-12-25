using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Interfaces.PashwordHashers
{
    public interface IPasswordHasher
    {
        public string HashPassword(string rawPassword);
        public bool VerifyPassword(string password, string passwordHash);
    }
}
