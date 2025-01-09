using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Classes.Commands.Edit;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Tests.Application.Features.Classes.Edit
{
    public class EditClassCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _mockClassRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly EditClassCommandHandler _handler;

        public EditClassCommandHandlerTests()
        {
            _mockClassRepository = new Mock<IClassRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new EditClassCommandHandler(_mockClassRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public void Handle_ShouldThrowClassRetrievalException_WhenClassNotFound()
        {
            // Arrange
            var command = new EditClassCommand { Id = Guid.NewGuid() };
            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync((Class?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassRetrievalException>().WithMessage($"Class with id {command.Id} not found.");
        }

        [Fact]
        public void Handle_ShouldThrowClassPersistenceException_WhenClassNameAlreadyExist()
        {
            // Arrange
            var command = new EditClassCommand
            {
                Id = Guid.NewGuid(),
                Name = "Class"
            };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _mockClassRepository.Setup(x => x.GetByNameAsync(command.Name, default)).ReturnsAsync(existingClass);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassPersistenceException>().WithMessage($"Class with name {command.Name} already exist.");
        }

        [Fact]
        public void Handle_ShouldThrowClassPersistenceException_WhenInvalidDateRange()
        {
            // Arrange
            var command = new EditClassCommand
            {
                Id = Guid.NewGuid(),
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                EndDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassPersistenceException>().WithMessage($"Cannot update class with id {command.Id} because of invalid date range.");
        }

        [Fact]
        public void Handle_ShouldThrowClassPersistenceException_WhenClassCapacityLessThan20()
        {
            // Arrange
            var command = new EditClassCommand
            {
                Id = Guid.NewGuid(),
                MaxCapacity = 10
            };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassPersistenceException>().WithMessage($"Cannot update class with id {command.Id} because class capacity must be greater or equal 20.");
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenFailedToSaveChanges()
        {
            // Arrange
            var command = new EditClassCommand
            {
                Id = Guid.NewGuid(),
                MaxCapacity = 30
            };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassPersistenceException>().WithMessage($"Failed to save changes while updating class with id {command.Id}");
        }

        [Fact]
        public async Task Handle_ShouldReturnId_WhenClassEditedSuccessfully()
        {
            // Arrange
            var command = new EditClassCommand
            {
                Id = Guid.NewGuid(),
                Name = "Class",
                Description = "Description",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Status = ClassStatus.ACTIVE,
                MaxCapacity = 30
            };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _mockClassRepository.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _mockClassRepository.Setup(x => x.GetByNameAsync(command.Name, default)).ReturnsAsync((Class?)null);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Should().NotBeEmpty();
        }
    }
}
