using FluentAssertions;
using Moq;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Application.Features.Students.Commands.Edit;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Tests.Application.Features.Students.Edit
{
    public class EditStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly EditStudentCommandHandler _handler;
        private readonly Mock<IOutboxRepository> _outboxRepository;

        public EditStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _outboxRepository = new Mock<IOutboxRepository>();
            _handler = new EditStudentCommandHandler(_studentRepositoryMock.Object, _outboxRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = new EditStudentCommand { Id = Guid.NewGuid() };
            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default)).ReturnsAsync((Student?)null);
            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            // Assert
            act.Should().ThrowAsync<StudentRetrievalException>().WithMessage($"Student with id {command.Id} do not exist");
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenEmailAlreadyExist()
        {
            // Arrange
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

            var command = new EditStudentCommand
            {
                Id = existingStudent.Id,
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 8, 8)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "Ward", "District", "City"),
                ExposePrivateInfo = false
            };

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(existingStudent.Id, default)).ReturnsAsync(existingStudent);
            _studentRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, default)).ReturnsAsync(existingStudent);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<StudentPersistenceException>().WithMessage($"Student with email {command.Email} already exist.");
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenSaveChangesFails()
        {
            // Arrange
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

            var command = new EditStudentCommand
            {
                Id = existingStudent.Id,
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 8, 8)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "Ward", "District", "City"),
                ExposePrivateInfo = true
            };

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(existingStudent.Id, default)).ReturnsAsync(existingStudent);
            _unitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<StudentPersistenceException>().WithMessage($"Failed to save changes while updating student.");

        }

        [Fact]
        public async Task Handler_ShouldReturnStudentId_WhenEditStudentSuccessfullyAsync()
        {
            // Arrange
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

            var command = new EditStudentCommand
            {
                Id = existingStudent.Id,
                FirstName = "Tin",
                LastName = "Vo",
                Email = "votrongtin882003@gmail.com",
                Gender = GenderEnum.MALE,
                DateOfBirth = DateOnly.FromDateTime(new DateTime(2003, 8, 8)),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Address = new Address("123", "Street", "Ward", "District", "City"),
                ExposePrivateInfo = true
            };

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(existingStudent.Id, default)).ReturnsAsync(existingStudent);
            _unitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(existingStudent.Id);
        }
    }
}
