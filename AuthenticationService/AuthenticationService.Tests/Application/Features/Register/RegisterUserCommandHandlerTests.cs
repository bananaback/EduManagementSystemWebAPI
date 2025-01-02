using AuthenticationService.Application.Common.Interfaces;
using AuthenticationService.Application.Common.Interfaces.PashwordHashers;
using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Application.Features.Register;
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

namespace AuthenticationService.Tests.Application.Features.Register
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RegisterUserCommandHandler(_passwordHasherMock.Object, _userRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicatedUserException_WhenUserWithUsernameExists()
        {
            // Arrange
            var command = new RegisterUserCommand { Username = "existingUser", Password = "$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu" };
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApplicationUser(Username.Create(command.Username), PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"), RoleEnum.User));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicatedUserException>()
                .WithMessage($"User with user name {command.Username} already exist.");
        }

        [Fact]
        public async Task Handle_ShouldReturnUserId_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var command = new RegisterUserCommand { Username = "newUser", Password = "Password123@" };
            var hashedPassword = "$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu";
            _passwordHasherMock.Setup(ph => ph.HashPassword(command.Password)).Returns(hashedPassword);
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApplicationUser?)null);  // No existing user
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);  // Simulate user creation
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);  // Simulate successful save

            var expectedUserId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .Callback<ApplicationUser, CancellationToken>((user, cancellationToken) =>
                {
                    user.Id = expectedUserId;
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedUserId);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowUserPersistenceException_WhenSaveChangesFails()
        {
            // Arrange
            var command = new RegisterUserCommand { Username = "newUser", Password = "P@assword123" };
            var hashedPassword = "$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu";
            _passwordHasherMock.Setup(ph => ph.HashPassword(command.Password)).Returns(hashedPassword);
            _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApplicationUser?)null);  // No existing user
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);  // Simulate user creation
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);  // Simulate failure to save changes

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UserPersistenceException>()
                .WithMessage("Failed to save changes when creating user.");
        }
    }
}
