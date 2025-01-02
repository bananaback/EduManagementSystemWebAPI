using AuthenticationService.Infrastructure.Services.TokenGenerators;
using AuthenticationService.Application.Common.Interfaces.TokenGenerators;
using FluentAssertions;
using Moq;
using System.Security.Claims;

namespace AuthenticationService.Tests.Infrastructure.Services.TokenGenerators
{
    public class RefreshTokenGeneratorTests
    {
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly Mock<ITokenGenerator> _mockTokenGenerator;
        private readonly AuthenticationConfiguration _configuration;

        public RefreshTokenGeneratorTests()
        {
            // Mock the ITokenGenerator dependency
            _mockTokenGenerator = new Mock<ITokenGenerator>();

            // Configuration mock
            _configuration = new AuthenticationConfiguration
            {
                RefreshTokenSecret = "SuperSecretKey",
                Issuer = "https://myapp.com",
                Audiences = new List<string> { "https://audience1.com", "https://audience2.com" },
                RefreshTokenExpirationMinutes = 60
            };

            _refreshTokenGenerator = new RefreshTokenGenerator(_configuration, _mockTokenGenerator.Object);
        }

        [Fact]
        public void GenerateToken_ShouldReturnToken()
        {
            // Arrange
            string expectedToken = "dummy_token";
            _mockTokenGenerator
                .Setup(t => t.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<double>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns(expectedToken);

            // Act
            string token = _refreshTokenGenerator.GenerateToken();

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().Be(expectedToken);
            _mockTokenGenerator.Verify(t => t.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audiences,
                _configuration.RefreshTokenExpirationMinutes,
                It.IsAny<IEnumerable<Claim>>()
            ), Times.Once);
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenConfigurationIsNull()
        {
            // Act
            Action act = () => new RefreshTokenGenerator(null!, _mockTokenGenerator.Object).GenerateToken();

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenTokenGeneratorIsNull()
        {
            // Act
            Action act = () => new RefreshTokenGenerator(_configuration, null!).GenerateToken();

            // Assert
            act.Should().Throw<NullReferenceException>();
        }
    }
}
