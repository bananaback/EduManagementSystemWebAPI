using AuthenticationService.Infrastructure.Services.TokenGenerators;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthenticationService.Tests.Infrastructure.Services.TokenGenerators
{
    public class TokenGeneratorTests
    {
        private readonly TokenGenerator _tokenGenerator;

        public TokenGeneratorTests()
        {
            _tokenGenerator = new TokenGenerator();
        }

        [Fact]
        public void GenerateToken_ShouldGenerateValidJwtToken()
        {
            // Arrange
            string secretKey = "xzprm09FqRCyBPzd0sIQ3JAfmvxa8uHmkeUw1bSapUtxfhHj0VY8DjkpZBZgWO6jmlgBrD6Ac10idL7EEzXUipOEfzFQv";
            string issuer = "https://localhost:7136";
            var audiences = new List<string> { "https://localhost:7136", "https://localhost:7136", "https://localhost:7136" };
            double expirationMinutes = 10;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Act
            string token = _tokenGenerator.GenerateToken(secretKey, issuer, audiences, expirationMinutes, claims);

            // Assert
            token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(token).Should().BeTrue();

            var jwtToken = handler.ReadJwtToken(token);
            jwtToken.Issuer.Should().Be(issuer);
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "TestUser");
            jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

            foreach (var audience in audiences)
            {
                jwtToken.Claims.Should().Contain(c => c.Type == "aud" && c.Value == audience);
            }

            jwtToken.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(expirationMinutes), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void GenerateToken_ShouldHandleNullClaims()
        {
            // Arrange
            string secretKey = "xzprm09FqRCyBPzd0sIQ3JAfmvxa8uHmkeUw1bSapUtxfhHj0VY8DjkpZBZgWO6jmlgBrD6Ac10idL7EEzXUipOEfzFQv";
            string issuer = "https://localhost:7136";
            var audiences = new List<string> { "https://localhost:7136" };
            double expirationMinutes = 15;

            // Act
            string token = _tokenGenerator.GenerateToken(secretKey, issuer, audiences, expirationMinutes, null);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            jwtToken.Claims.Should().Contain(c => c.Type == "aud" && c.Value == audiences[0]);
        }

        [Fact]
        public void GenerateToken_ShouldThrowExceptionForInvalidSecretKey()
        {
            // Arrange
            string secretKey = ""; // Invalid empty secret key
            string issuer = "https://localhost:7136";
            var audiences = new List<string> { "https://localhost:7136" };
            double expirationMinutes = 10;
            var claims = new List<Claim>();

            // Act
            Action act = () => _tokenGenerator.GenerateToken(secretKey, issuer, audiences, expirationMinutes, claims);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
