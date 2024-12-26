using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Services.TokenGenerators
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; } = string.Empty;
        public string RefreshTokenSecret { get; set; } = string.Empty;
        public double AccessTokenExpirationMinutes { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public List<string> Audiences { get; set; } = new List<string>();
    }
}
