using AuthenticationService.Infrastructure.Services.TokenValidators;
using FluentAssertions;
using Moq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Infrastructure.Services.TokenGenerators;

namespace AuthenticationService.Tests.Infrastructure.Services.TokenValidators
{
    public class RefreshTokenValidatorTests
    {
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly AuthenticationConfiguration _mockConfiguration;

        public RefreshTokenValidatorTests()
        {
            // Mock the AuthenticationConfiguration dependency
            _mockConfiguration = new AuthenticationConfiguration();

            // Configuration mock setup
            _mockConfiguration.RefreshTokenSecret = "8LaXTXvjx9m7LWccC673qQmFT6iNy6T2xR8cMs8aVgsx4DwL0LvKh7E7vY7f8QqVTjJszk--RcfUHoN61ihhgDOyyBDJtnJ1Y9g3RBj86ZiYhsXzxLG5y37a-CavPEgSNq1IWPVkfc00TBiAsa9zdkI-fObjfINSKBO1tQAPB7E";
            _mockConfiguration.Issuer = "https://localhost:7136";
            _mockConfiguration.Audiences = new List<string> { "https://audience1.com", "https://audience2.com" };

            _refreshTokenValidator = new RefreshTokenValidator(_mockConfiguration);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_ForValidToken()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_mockConfiguration.RefreshTokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "testuser")
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = _mockConfiguration.Issuer,
                Audience = _mockConfiguration.Audiences.First(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var validToken = tokenHandler.WriteToken(token);

            // Act
            var result = _refreshTokenValidator.Validate(validToken);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldReturnFalse_ForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid_token";

            // Act
            var result = _refreshTokenValidator.Validate(invalidToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_ShouldReturnFalse_ForExpiredToken()
        {
            // Arrange
            var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU";

            // Act
            var result = _refreshTokenValidator.Validate(expiredToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_ShouldReturnFalse_ForTokenWithInvalidSignature()
        {
            // Arrange

            var invalidSignatureToken = "invalid_signature";

            // Act
            var result = _refreshTokenValidator.Validate(invalidSignatureToken);

            // Assert
            result.Should().BeFalse();
        }
    }
}
