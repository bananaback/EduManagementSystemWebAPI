using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Features.Classes.Queries.GetAll;
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

namespace ClassManagement.Tests.Application.Features.Classes.GetAll
{
    public class GetAllClassesCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _classRepositoryMock;

        public GetAllClassesCommandHandlerTests()
        {
            _classRepositoryMock = new Mock<IClassRepository>();
        }

        [Fact]
        public void Handle_ShouldReturnClasses()
        {
            // Arrange
            var command = new GetAllClassesCommand();
            var handler = new GetAllClassesCommandHandler(_classRepositoryMock.Object);
            var classes = new List<Class>
            {
                new Class
                (
                    new ClassName("Class one"),
                    new ClassDescription("Description 1"),
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    ClassStatus.ACTIVE,
                    45
                ),
                new Class
                (
                    new ClassName("Class two"),
                    new ClassDescription("Description 2"),
                    DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    ClassStatus.ACTIVE,
                    45
                )
            };
            _classRepositoryMock.Setup(x => x.SearchAsync(
                command.PageNumber!.Value,
                command.ItemsPerPage!.Value,
                command.Id,
                command.Name,
                command.Description,
                command.StartDate,
                command.EndDate,
                command.Status,
                command.MaxCapacity,
                It.IsAny<CancellationToken>())).ReturnsAsync(classes);

            // Act
            var result = handler.Handle(command, CancellationToken.None).Result;

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Id.Should().NotBeEmpty();
            result.First().Name.Should().Be("Class one");
            result.First().Description.Should().Be("Description 1");
            result.First().StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.First().EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
            result.First().ClassStatus.Should().Be(ClassStatus.ACTIVE);
            result.First().MaxCapacity.Should().Be(45);
            result.Last().Id.Should().NotBeEmpty();
            result.Last().Name.Should().Be("Class two");
            result.Last().Description.Should().Be("Description 2");
            result.Last().StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.Last().EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
            result.Last().ClassStatus.Should().Be(ClassStatus.ACTIVE);
            result.Last().MaxCapacity.Should().Be(45);
        }
    }
}
