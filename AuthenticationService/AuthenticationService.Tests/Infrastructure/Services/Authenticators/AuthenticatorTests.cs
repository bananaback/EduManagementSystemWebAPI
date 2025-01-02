using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Infrastructure.Services.Authenticators;
using AuthenticationService.Infrastructure.Services.TokenGenerators;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AuthenticationService.Tests.Infrastructure.Services.Authenticators
{
    public class AuthenticatorTests
    {
        private readonly Authenticator _authenticator;
        private readonly Mock<AccessTokenGenerator> _mockAccessTokenGenerator;
        private readonly Mock<RefreshTokenGenerator> _mockRefreshTokenGenerator;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public AuthenticatorTests()
        {
            _mockAccessTokenGenerator = new Mock<AccessTokenGenerator>();
            _mockRefreshTokenGenerator = new Mock<RefreshTokenGenerator>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _authenticator = new Authenticator(
                _mockAccessTokenGenerator.Object,
                _mockRefreshTokenGenerator.Object,
                _mockRefreshTokenRepository.Object,
                _mockUnitOfWork.Object
            );
        }

        [Fact]
        public async Task Authenticate_ShouldReturnAuthenticatedUserResult_WhenSuccessful()
        {
            // Arrange
            var user = new ApplicationUser(Username.Create("bananaback"), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User);
            var accessToken = "access_token";
            var refreshTokenString = "refresh_token";
            var refreshToken = new RefreshToken
            {
                User = user,
                Token = TokenValue.Create(refreshTokenString)
            };

            _mockAccessTokenGenerator.Setup(x => x.GenerateToken(user)).Returns(accessToken);
            _mockRefreshTokenGenerator.Setup(x => x.GenerateToken()).Returns(refreshTokenString);
            _mockRefreshTokenRepository.Setup(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _authenticator.Authenticate(user);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken!.Value.Should().Be(accessToken);
            result.RefreshToken!.Value.Should().Be(refreshTokenString);
            _mockRefreshTokenRepository.Verify(x => x.AddAsync(It.Is<RefreshToken>(rt => rt.Token.Value == refreshTokenString && rt.User == user), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Authenticate_ShouldThrowTokenPersistenceException_WhenSaveChangesFails()
        {
            // Arrange
            var user = new ApplicationUser(Username.Create("bananaback"), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User);
            var accessToken = "access_token";
            var refreshTokenString = "refresh_token";

            _mockAccessTokenGenerator.Setup(x => x.GenerateToken(user)).Returns(accessToken);
            _mockRefreshTokenGenerator.Setup(x => x.GenerateToken()).Returns(refreshTokenString);
            _mockRefreshTokenRepository.Setup(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _authenticator.Authenticate(user);

            // Assert
            await act.Should().ThrowAsync<TokenPersistenceException>().WithMessage("Failed to save refres token while authenticate user.");
            _mockRefreshTokenRepository.Verify(x => x.AddAsync(It.Is<RefreshToken>(rt => rt.Token.Value == refreshTokenString && rt.User == user), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
