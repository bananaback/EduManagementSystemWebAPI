using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Features.LogoutUser;
using AuthenticationService.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Application.Features.LogoutUser
{
    public class LogoutUserAllDevicesCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly LogoutUserAllDevicesCommandHandler _handler;

        public LogoutUserAllDevicesCommandHandlerTests()
        {
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new LogoutUserAllDevicesCommandHandler(_refreshTokenRepositoryMock.Object, _unitOfWorkMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenRefreshTokenNotFound()
        {
            // Arrange
            var command = new LogoutUserAllDevicesCommand("nonexistent_token");
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
            var command = new LogoutUserAllDevicesCommand("valid_token");
            var refreshToken = new RefreshToken { Token = command.RefreshToken };

            _refreshTokenRepositoryMock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);
            _refreshTokenRepositoryMock.Setup(repo => repo.DeleteAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0); // Simulate failure

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _refreshTokenRepositoryMock.Verify(repo => repo.DeleteAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenLogoutIsSuccessful()
        {
            // Arrange
            var command = new LogoutUserAllDevicesCommand("valid_token");
            var refreshToken = new RefreshToken { Token = command.RefreshToken };

            _refreshTokenRepositoryMock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(refreshToken);
            _refreshTokenRepositoryMock.Setup(repo => repo.DeleteAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate success

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _refreshTokenRepositoryMock.Verify(repo => repo.DeleteAllByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
