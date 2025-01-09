using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Application.Features.Classes.Queries;
using ClassManagement.Application.Features.Classes.Queries.GetById;
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

namespace ClassManagement.Tests.Application.Features.Classes.GetById
{
    public class GetClassByIdCommandHandlerTests
    {
        private readonly Mock<IClassRepository> _classRepositoryMock;
        private readonly GetClassByIdCommandHandler _handler;

        public GetClassByIdCommandHandlerTests()
        {
            _classRepositoryMock = new Mock<IClassRepository>();
            _handler = new GetClassByIdCommandHandler(_classRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenClassNotFound()
        {
            // Arrange
            var command = new GetClassByIdCommand
            {
                Id = Guid.NewGuid()
            };

            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default))
                .ReturnsAsync((Class?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            act.Should().ThrowAsync<ClassRetrievalException>();
        }

        [Fact]
        public async void Handle_ShouldReturnClass()
        {
            // Arrange
            var command = new GetClassByIdCommand
            {
                Id = Guid.NewGuid()
            };

            var handler = new GetClassByIdCommandHandler(_classRepositoryMock.Object);
            var @class = new Class
            (
                new ClassName("Class one"),
                new ClassDescription("Description 1"),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                ClassStatus.ACTIVE,
                45
            );

            _classRepositoryMock.Setup(x => x.GetByIdAsync(command.Id, default))
                .ReturnsAsync(@class);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().BeOfType<ClassReadDto>();
        }
    }
}
