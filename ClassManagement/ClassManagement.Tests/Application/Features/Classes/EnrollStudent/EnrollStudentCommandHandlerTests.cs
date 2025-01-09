using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Classes.Commands.EnrollStudent;
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

namespace ClassManagement.Tests.Application.Features.Classes.EnrollStudent
{
    public class EnrollStudentCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _classRepositoryMock;
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly EnrollStudentCommandHandler _handler;

        public EnrollStudentCommandHandlerTests()
        {
            _classRepositoryMock = new Mock<IClassRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new EnrollStudentCommandHandler(_classRepositoryMock.Object, _studentRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenClassIsNotFound()
        {
            // Arrange
            var command = new EnrollStudentCommand { ClassId = Guid.NewGuid() };
            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.ClassId, default)).ReturnsAsync((ClassManagement.Domain.Entities.Class?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            act.Should().ThrowAsync<ClassRetrievalException>().WithMessage($"Class with id {command.ClassId} not found.");
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenStudentIsNotFound()
        {
            // Arrange
            var command = new EnrollStudentCommand { StudentId = Guid.NewGuid() };
            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, default)).ReturnsAsync((ClassManagement.Domain.Entities.Student?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            act.Should().ThrowAsync<StudentRetrievalException>().WithMessage($"Student with id {command.StudentId} not found.");
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenFailedToSaveChanges()
        {
            // Arrange
            var command = new EnrollStudentCommand { StudentId = Guid.NewGuid(), ClassId = Guid.NewGuid() };

            var student = new Student
            (
                new PersonName("John", "Doe"),
                new Email("johndoe@gmail.com"),
                GenderEnum.MALE,
                DateOnly.FromDateTime(new DateTime(2000, 02, 12)),
                DateOnly.FromDateTime(DateTime.Now),
                new Address("20", "A", "B", "C", "D"),
                false
            );

            var @class = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, default)).ReturnsAsync(student);
            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.ClassId, default)).ReturnsAsync(@class);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            act.Should().ThrowAsync<StudentEnrollmentException>().WithMessage($"Failed to save changes while trying to enroll student with id {command.StudentId} into class with id {command.ClassId}.");
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenStudentEnrolledSuccessfullyAsync()
        {
            // Arrange
            var command = new EnrollStudentCommand { StudentId = Guid.NewGuid(), ClassId = Guid.NewGuid() };

            var student = new Student
            (
                new PersonName("John", "Doe"),
                new Email("johndoe@gmail.com"),
                GenderEnum.MALE,
                DateOnly.FromDateTime(new DateTime(2000, 02, 12)),
                DateOnly.FromDateTime(DateTime.Now),
                new Address("20", "A", "B", "C", "D"),
                false
            );

            var @class = new Class
            (
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                30
            );

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, default)).ReturnsAsync(student);
            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.ClassId, default)).ReturnsAsync(@class);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
        }
    }
}
