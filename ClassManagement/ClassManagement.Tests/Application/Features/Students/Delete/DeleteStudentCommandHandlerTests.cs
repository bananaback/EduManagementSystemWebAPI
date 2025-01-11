﻿using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Students.Commands.Delete;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ClassManagement.Tests.Application.Features.Students.Delete
{
    public class DeleteStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly DeleteStudentCommandHandler _handler;

        public DeleteStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteStudentCommandHandler(_studentRepositoryMock.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenStudentNotFound()
        {
            // Arrange
            var command = new DeleteStudentCommand { Id = Guid.NewGuid() };
            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Student?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<StudentRetrievalException>().WithMessage($"Student with id {command.Id} not exist.");
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenSaveChangesFails()
        {
            // Arrange
            var command = new DeleteStudentCommand { Id = Guid.NewGuid() };
            var existingStudent = new Student
                        (
                            new PersonName("Tin", "Vo"),
                            new Email("votrongtin882003@gmail.com"),
                            GenderEnum.MALE,
                            DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                            DateOnly.FromDateTime(DateTime.Now),
                            new Address("123", "Street", "City", "Country", "ZipCode"),
                            false
                        );
            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingStudent);
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<StudentPersistenceException>().WithMessage($"Failed to save changes while deleting student with id {command.Id}");
        }

        [Fact]
        public async Task Handler_ShouldReturnStudentId_WhenDeleteStudentSuccessfullyAsync()
        {
            // Arrange
            var command = new DeleteStudentCommand { Id = Guid.NewGuid() };
            var existingStudent = new Student
            (
                new PersonName("Tin", "Vo"),
                new Email("votrongtin882003@gmail.com"),
                GenderEnum.MALE,
                DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                DateOnly.FromDateTime(DateTime.Now),
                new Address("123", "Street", "City", "Country", "ZipCode"),
                false
            );

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingStudent);
            _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(command.Id);
        }
    }
}
