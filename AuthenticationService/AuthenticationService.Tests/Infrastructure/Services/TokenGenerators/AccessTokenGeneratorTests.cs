using AuthenticationService.Infrastructure.Services.TokenGenerators;
using AuthenticationService.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Application.Common.Interfaces.TokenGenerators;

namespace AuthenticationService.Tests.Infrastructure.Services.TokenGenerators
{
    public class AccessTokenGeneratorTests
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly Mock<ITokenGenerator> _mockTokenGenerator;
        private readonly AuthenticationConfiguration _configuration;

        public AccessTokenGeneratorTests()
        {
            // Mock the TokenGenerator dependency
            _mockTokenGenerator = new Mock<ITokenGenerator>();

            // Configuration mock
            _configuration = new AuthenticationConfiguration
            {
                AccessTokenSecret = "SuperSecretKey",
                Issuer = "https://myapp.com",
                Audiences = new List<string> { "https://audience1.com", "https://audience2.com" },
                AccessTokenExpirationMinutes = 15
            };

            _accessTokenGenerator = new AccessTokenGenerator(_configuration, _mockTokenGenerator.Object);
        }

        [Fact]
        public void GenerateToken_ShouldIncludeAllClaims()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            string expectedToken = "dummy_token";
            _mockTokenGenerator
                .Setup(t => t.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<double>(), It.IsAny<List<Claim>>()))
                .Returns(expectedToken);

            // Act
            string token = _accessTokenGenerator.GenerateToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
            _mockTokenGenerator.Verify(t => t.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audiences,
                _configuration.AccessTokenExpirationMinutes,
                It.Is<List<Claim>>(claims =>
                    claims.Exists(c => c.Type == "id" && c.Value == user.Id.ToString()) &&
                    claims.Exists(c => c.Type == ClaimTypes.Name && c.Value == user.Username.Value) &&
                    claims.Exists(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString())
                )
            ), Times.Once);
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenConfigurationIsNull()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            // Act
            Action act = () => new AccessTokenGenerator(null!, _mockTokenGenerator.Object).GenerateToken(user);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void GenerateToken_ShouldThrowException_WhenTokenGeneratorIsNull()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            // Act
            Action act = () => new AccessTokenGenerator(_configuration, null!).GenerateToken(user);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

    }
}
