using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Common.Interfaces.TokenValidators;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Features.RotateToken;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Application.Features.RotateToken
{
    public class RotateTokenCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITokenValidator> _tokenValidatorMock;
        private readonly Mock<IAuthenticator> _authenticatorMock;
        private readonly RotateTokenCommandHandler _handler;

        public RotateTokenCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tokenValidatorMock = new Mock<ITokenValidator>();
            _authenticatorMock = new Mock<IAuthenticator>();

            _handler = new RotateTokenCommandHandler(
                _userRepositoryMock.Object,
                _refreshTokenRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _tokenValidatorMock.Object,
                _authenticatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTokenIsInvalid()
        {
            // Arrange
            var command = new RotateTokenCommand("refresh_token");
            _tokenValidatorMock.Setup(tokenValidator => tokenValidator.Validate(command.RefreshToken.Value))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeFalse();
            _tokenValidatorMock.Verify(tl => tl.Validate(It.IsAny<string>()), Times.Once);
            _refreshTokenRepositoryMock.Verify(rp => rp.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _userRepositoryMock.Verify(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTokenIsRevoked()
        {
            // Arrange
            var command = new RotateTokenCommand("refresh_token");
            _tokenValidatorMock.Setup(tokenValidator => tokenValidator.Validate(command.RefreshToken.Value))
                .Returns(true);
            _refreshTokenRepositoryMock.Setup(rp => rp.GetByTokenAsync(command.RefreshToken.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            _tokenValidatorMock.Verify(tl => tl.Validate(It.IsAny<string>()), Times.Once);
            _refreshTokenRepositoryMock.Verify(rp => rp.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowUserRetrievalException_WhenUserNotFound()
        {
            // Arrange
            var command = new RotateTokenCommand("refresh_token");
            var user = new ApplicationUser(Username.Create("bananaback"), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User);
            _tokenValidatorMock.Setup(tokenValidator => tokenValidator.Validate(command.RefreshToken.Value))
               .Returns(true);
            _refreshTokenRepositoryMock.Setup(rp => rp.GetByTokenAsync(command.RefreshToken.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken(user, It.IsAny<TokenValue>()));
            _userRepositoryMock.Setup(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async() => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UserRetrievalException>();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAuthenticateFailed()
        {
            // Arrange
            var command = new RotateTokenCommand("refresh_token");
            var user = new ApplicationUser(Username.Create("bananaback"), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User);
            _tokenValidatorMock.Setup(tokenValidator => tokenValidator.Validate(command.RefreshToken.Value))
               .Returns(true);
            _refreshTokenRepositoryMock.Setup(rp => rp.GetByTokenAsync(command.RefreshToken.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken(user, It.IsAny<TokenValue>()));
            _userRepositoryMock.Setup(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _authenticatorMock.Setup(a => a.Authenticate(user, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthenticatedUserResult.Failure());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            _tokenValidatorMock.Verify(tl => tl.Validate(It.IsAny<string>()), Times.Once);
            _refreshTokenRepositoryMock.Verify(rp => rp.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _authenticatorMock.Verify(a => a.Authenticate(user, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenRotateTokenSuccess()
        {
            // Arrange
            var command = new RotateTokenCommand("refresh_token");
            var user = new ApplicationUser(Username.Create("bananaback"), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User);
            _tokenValidatorMock.Setup(tokenValidator => tokenValidator.Validate(command.RefreshToken.Value))
               .Returns(true);
            _refreshTokenRepositoryMock.Setup(rp => rp.GetByTokenAsync(command.RefreshToken.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RefreshToken(user, It.IsAny<TokenValue>()));
            _userRepositoryMock.Setup(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _authenticatorMock.Setup(a => a.Authenticate(user, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthenticatedUserResult.Success("access_token", "refresh_token"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _tokenValidatorMock.Verify(tl => tl.Validate(It.IsAny<string>()), Times.Once);
            _refreshTokenRepositoryMock.Verify(rp => rp.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(up => up.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _authenticatorMock.Verify(a => a.Authenticate(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
