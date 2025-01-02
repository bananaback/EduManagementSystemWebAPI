using AuthenticationService.Application.Common.Interfaces.TokenGenerators;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerators
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, List<string> audiences, double exprirationMinutes, IEnumerable<Claim> claims)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            claims = claims ?? new List<Claim>();  // Ensure a non-null list
            var claimList = claims.ToList();

            foreach (string audience in audiences)
            {
                Claim audClaim = new Claim("aud", audience);
                claimList.Add(audClaim);
            }

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                null,
                claimList,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(exprirationMinutes),
                credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
