using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Interfaces.TokenGenerators
{
    public interface ITokenGenerator
    {
        string GenerateToken(string secretKey, string issuer, List<string> audiences, double exprirationMinutes, IEnumerable<Claim> claims);
    }

}
