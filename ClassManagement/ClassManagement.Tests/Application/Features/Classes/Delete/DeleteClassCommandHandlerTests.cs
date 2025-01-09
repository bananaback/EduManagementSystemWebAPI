using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Classes.Commands.Delete;
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

namespace ClassManagement.Tests.Application.Features.Classes.Delete
{
    public class DeleteClassCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _classRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteClassCommandHandler _handler;

        public DeleteClassCommandHandlerTests()
        {
            _classRepositoryMock = new Mock<IClassRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteClassCommandHandler(_classRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenStudentNotFound()
        {
            // Arrange
            var command = new DeleteClassCommand { Id = Guid.NewGuid() };
            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync((ClassManagement.Domain.Entities.Class?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassRetrievalException>().WithMessage($"Class with id {command.Id} not found.");
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenFailedToSaveChanges()
        {
            // Arrange
            var command = new DeleteClassCommand { Id = Guid.NewGuid() };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<ClassPersistenceException>().WithMessage($"Failed to save changes while deleting class with id {command.Id}");
        }

        [Fact]
        public async Task Handle_ShouldReturnClassId_WhenClassDeletedSuccessfullyAsync()
        {
            // Arrange
            var command = new DeleteClassCommand { Id = Guid.NewGuid() };

            var existingClass = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync(existingClass);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command);

            // Assert
            result.Should().NotBeEmpty();
        }
    }
}
