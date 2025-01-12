using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Enrollments.Commands.Update;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ClassManagement.Tests.Application.Features.Enrollments.Update
{
    public class UpdateEnrollmentCommandHandlerTests
    {
        private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateEnrollmentCommandHandler _handler;

        public UpdateEnrollmentCommandHandlerTests()
        {
            _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateEnrollmentCommandHandler(_enrollmentRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Handler_ShouldThrowException_WhenEnrollmentNotFound()
        {
            // Arrange
            var command = new UpdateEnrollmentCommand
            {
                StudentId = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                Grade = new Grade("A"),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Status = EnrollmentStatus.ACTIVE
            };
            _enrollmentRepositoryMock.Setup(x => x.GetByClassIdAndStudentIdAsync(command.ClassId, command.StudentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Enrollment?)null);
            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            // Assert
            act.Should().ThrowAsync<EnrollmentRetrievalException>();
        }

        [Fact]
        public async Task Handler_ShouldUpdateEnrollmentAsync()
        {
            // Arrange
            var command = new UpdateEnrollmentCommand
            {
                StudentId = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                Grade = new Grade("A"),
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                Status = EnrollmentStatus.ACTIVE
            };

            var student = new Student
            (
                new PersonName("Tin", "Vo"),
                new Email("votrongtin882003@gmail.com"),
                GenderEnum.MALE,
                DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                DateOnly.FromDateTime(DateTime.Now),
                new Address("123", "Street", "City", "Country", "ZipCode"),
                false
            );

            var @class = new Class(
                new ClassName("Class"),
                new ClassDescription("Description"),
                DateOnly.FromDateTime(DateTime.Now.AddDays(12)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                ClassStatus.ACTIVE,
                30
            );

            var enrollment = new Enrollment(student, @class, DateOnly.FromDateTime(DateTime.Now));

            _enrollmentRepositoryMock.Setup(x => x.GetByClassIdAndStudentIdAsync(command.ClassId, command.StudentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.Should().BeTrue();
        }
    }
}
