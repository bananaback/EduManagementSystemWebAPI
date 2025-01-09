using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Classes.Commands.Create;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using ClassManagement.Infrastructure.Persistence;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Tests.Application.Features.Classes.Create
{
    public class CreateClassCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _classRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateClassCommandHandler _handler;

        public CreateClassCommandHandlerTests()
        {
            // Initialize the mocks
            _classRepositoryMock = new Mock<IClassRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Pass mocks to the handler
            _handler = new CreateClassCommandHandler(_classRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenClassWithNameAlreadyExist()
        {
            // Arrange
            var command = new CreateClassCommand
            {
                Name = "Class",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Status = ClassStatus.ACTIVE,
                MaxCapacity = 10
            };

            var existingClass = new Class(
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _classRepositoryMock.Setup(x => x.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClass);

            _unitOfWorkMock
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate successful save

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ClassCreationException>().WithMessage($"Class with name {command.Name} already exist.");
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenFailedToSaveChanges()
        {
            // Arrange
            var command = new CreateClassCommand
            {
                Name = "Class",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Status = ClassStatus.ACTIVE,
                MaxCapacity = 10
            };

            _classRepositoryMock.Setup(x => x.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Class?)null);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0); // Simulate failed save

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ClassCreationException>().WithMessage("Failed to save changes when creating new class");
        }

        [Fact]
        public void Handle_ShouldReturnClassId_WhenClassCreatedSuccessfully()
        {
            // Arrange
            var command = new CreateClassCommand
            {
                Name = "Class",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Status = ClassStatus.ACTIVE,
                MaxCapacity = 20
            };

            _classRepositoryMock.Setup(x => x.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Class?)null);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate successful save

            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            result.Should().NotBeEmpty();
        }
    }
}
