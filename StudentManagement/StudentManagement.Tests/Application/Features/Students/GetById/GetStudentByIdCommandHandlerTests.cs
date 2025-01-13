using FluentAssertions;
using Moq;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Application.Features.Students.Queries;
using StudentManagement.Application.Features.Students.Queries.GetById;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Tests.Application.Features.Students.GetById
{
    public class GetStudentByIdCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly GetStudentByIdCommandHandler _handler;
        public GetStudentByIdCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _handler = new GetStudentByIdCommandHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenStudentNotFound()
        {
            // Arrange
            var command = new GetStudentByIdCommand
            {
                Id = Guid.NewGuid()
            };
            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default))
                .ReturnsAsync((Student?)null);
            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);
            // Assert
            act.Should().ThrowAsync<StudentRetrievalException>();
        }

        [Fact]
        public async void Handle_ShouldReturnStudent()
        {
            // Arrange
            var command = new GetStudentByIdCommand
            {
                Id = Guid.NewGuid()
            };
            var handler = new GetStudentByIdCommandHandler(_studentRepositoryMock.Object);

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

            _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default))
                .ReturnsAsync(existingStudent);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeOfType<StudentReadDto>();
        }
    }
}
