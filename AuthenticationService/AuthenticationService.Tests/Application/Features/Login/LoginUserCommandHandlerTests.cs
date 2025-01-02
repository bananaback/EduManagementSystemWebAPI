using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Authenticators;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Domain.Enums;

namespace AuthenticationService.Tests.Application.Features.Login
{

    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IAuthenticator> _authenticatorMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _authenticatorMock = new Mock<IAuthenticator>();
            _handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _passwordHasherMock.Object,
                _authenticatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowUserRetrievalException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new LoginUserCommand { UserName = "nonexistentuser", Password = "Password123!" };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UserRetrievalException>()
                .WithMessage("User with username nonexistentuser not found.");
            _userRepositoryMock.Verify(repo => repo.GetByUsernameAsync(command.UserName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPasswordIsIncorrect()
        {
            // Arrange
            var command = new LoginUserCommand { UserName = "existinguser", Password = "WrongPassword99!" };
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            _passwordHasherMock.Verify(hasher => hasher.VerifyPassword(command.Password, user.HashPassword.Value), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var command = new LoginUserCommand { UserName = "validuser", Password = "ValidPassword100%" };
            var user = new ApplicationUser
            (
                Username.Create("validuser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var authResult = AuthenticatedUserResult.Success("access_token", "refresh_token");

            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            _authenticatorMock.Setup(auth => auth.Authenticate(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.AccessToken.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNull();
            _authenticatorMock.Verify(auth => auth.Authenticate(user, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}