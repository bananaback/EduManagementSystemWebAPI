using FluentAssertions;
using Moq;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Application.Features.Students.Commands.Create;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Tests.Application.Features.Students.Create
{
    public class CreateStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateStudentCommandHandler _handler;

        public CreateStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateStudentCommandHandler(_studentRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenStudentWithEmailAlreadyExist()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "City", "Country", "ZipCode"),
                ExposePrivateInfo = false
            };

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

            _studentRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingStudent);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<StudentCreationException>().WithMessage($"Student with email {command.Email} already exist.");
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenFailedToSaveChanges()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "City", "Country", "ZipCode"),
                ExposePrivateInfo = false
            };

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

            _studentRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingStudent);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command);

            // Assert
            act.Should().ThrowAsync<StudentCreationException>().WithMessage($"Failed to save changes while creating student.");
        }

        [Fact]
        public async Task Handler_ShouldReturnStudentId_WhenStudentCreatedSuccessfullyAsync()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "City", "Country", "ZipCode"),
                ExposePrivateInfo = false
            };

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

            _studentRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Student?)null);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
        }
    }
}
