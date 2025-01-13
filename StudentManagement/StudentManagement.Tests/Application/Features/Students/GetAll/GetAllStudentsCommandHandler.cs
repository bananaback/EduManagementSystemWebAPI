using FluentAssertions;
using Moq;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Features.Students.Queries.GetAll;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Tests.Application.Features.Students.GetAll
{
    public class GetAllStudentsCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly GetAllStudentsCommandHandler _handler;

        public GetAllStudentsCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _handler = new GetAllStudentsCommandHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnStudentsAsync()
        {
            // Arrange
            var command = new GetAllStudentsCommand();

            var students = new List<Student>
            {
                new Student
                (
                    new PersonName("Tin", "Vo"),
                    new Email("votrongtin882003@gmail.com"),
                    GenderEnum.MALE,
                    DateOnly.FromDateTime(new DateTime(2003, 08, 08)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("123", "Main Street", "Ward 1", "District 1", "HCMC"),
                    true
                ),
                new Student
                (
                    new PersonName("Anna", "Nguyen"),
                    new Email("anna.nguyen@example.com"),
                    GenderEnum.FEMALE,
                    DateOnly.FromDateTime(new DateTime(2001, 03, 15)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("456", "Highway Road", "Ward 5", "District 2", "Hanoi"),
                    true
                ),
                new Student
                (
                    new PersonName("John", "Smith"),
                    new Email("john.smith@example.com"),
                    GenderEnum.MALE,
                    DateOnly.FromDateTime(new DateTime(1999, 12, 25)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("789", "Broadway", "Ward 2", "District 3", "Da Nang"),
                    false
                ),
                new Student
                (
                    new PersonName("Emily", "Clark"),
                    new Email("emily.clark@example.com"),
                    GenderEnum.FEMALE,
                    DateOnly.FromDateTime(new DateTime(2000, 07, 20)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("101", "Ocean Avenue", "Ward 4", "District 4", "Nha Trang"),
                    true
                ),
                new Student
                (
                    new PersonName("Michael", "Brown"),
                    new Email("michael.brown@example.com"),
                    GenderEnum.MALE,
                    DateOnly.FromDateTime(new DateTime(1998, 11, 05)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("202", "Green Street", "Ward 3", "District 5", "Can Tho"),
                    false
                ),
                new Student
                (
                    new PersonName("Sophia", "Tran"),
                    new Email("sophia.tran@example.com"),
                    GenderEnum.FEMALE,
                    DateOnly.FromDateTime(new DateTime(2002, 04, 10)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("303", "Maple Road", "Ward 6", "District 6", "Hue"),
                    true
                ),
                new Student
                (
                    new PersonName("David", "Nguyen"),
                    new Email("david.nguyen@example.com"),
                    GenderEnum.MALE,
                    DateOnly.FromDateTime(new DateTime(2001, 01, 01)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("404", "River Street", "Ward 7", "District 7", "Vung Tau"),
                    false
                ),
                new Student
                (
                    new PersonName("Linda", "Johnson"),
                    new Email("linda.johnson@example.com"),
                    GenderEnum.FEMALE,
                    DateOnly.FromDateTime(new DateTime(2000, 09, 09)),
                    DateOnly.FromDateTime(DateTime.Now),
                    new Address("505", "Hilltop Lane", "Ward 8", "District 8", "Phu Quoc"),
                    true
                )
            };

            _studentRepositoryMock.Setup(x => x.GetAllAsync(
                command.PageNumber!.Value,
                command.ItemsPerPage!.Value,
                command.Id,
                command.FirstName,
                command.LastName,
                command.Email,
                command.DateOfBirthBefore,
                command.DateOfBirthAfter,
                command.EnrollmentDateBefore,
                command.EnrollmentDateAfter,
                command.HouseNumber,
                command.Street,
                command.Ward,
                command.District,
                command.City,
                It.IsAny<CancellationToken>())).ReturnsAsync(students);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(8);
            result.First().Id.Should().NotBeEmpty();
            result.First().Name.Should().Be("Tin Vo");
            result.First().Email.Should().Be("votrongtin882003@gmail.com");
            result.First().DateOfBirth.Should().Be(DateOnly.FromDateTime(new DateTime(2003, 08, 08)));
            result.First().DateEnrolled.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.First().Address.Should().Be("123, Main Street, Ward 1, District 1, HCMC");
        }
    }
}
