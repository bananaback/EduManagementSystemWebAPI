using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Features.LogoutUser;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Application.Features.LogoutUser
{
    public class LogoutUserCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly LogoutUserCommandHandler _handler;

        public LogoutUserCommandHandlerTests()
        {
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new LogoutUserCommandHandler(_refreshTokenRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenRefreshTokenNotFound()
        {
            // Arrange
            var command = new LogoutUserCommand("nonexistent_token");
            _refreshTokenRepositoryMock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _refreshTokenRepositoryMock.Verify(repo => repo.GetByTokenAsync(command.RefreshToken.Value, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenSaveChangesFails()
        {
            // Arrange
            var command = new LogoutUserCommand("valid_token");
            var refreshToken = new RefreshToken { Token = command.RefreshToken };

            _refreshTokenRepositoryMock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);
            _refreshTokenRepositoryMock.Setup(repo => repo.Delete(It.IsAny<RefreshToken>()));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0); // Simulate failure

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _refreshTokenRepositoryMock.Verify(repo => repo.Delete(refreshToken), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenLogoutIsSuccessful()
        {
            // Arrange
            var command = new LogoutUserCommand("valid_token");
            var refreshToken = new RefreshToken { Token = command.RefreshToken };

            _refreshTokenRepositoryMock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);
            _refreshTokenRepositoryMock.Setup(repo => repo.Delete(It.IsAny<RefreshToken>()));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate success

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _refreshTokenRepositoryMock.Verify(repo => repo.Delete(refreshToken), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
