using AuthenticationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AuthenticationService.Tests.Infrastructure.Persistence
{
    public class UnitOfWorkTests
    {
        private readonly Mock<AuthenticationDbContext> _mockContext;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockContext = new Mock<AuthenticationDbContext>(
                new DbContextOptionsBuilder<AuthenticationDbContext>()
                    .UseInMemoryDatabase("TestDatabase_UnitOfWork")
                    .Options
            );
            _unitOfWork = new UnitOfWork(_mockContext.Object);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldCallSaveChangesOnDbContext()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            _mockContext
                .Setup(c => c.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1) // Simulate one change saved
                .Verifiable();

            // Act
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Assert
            Assert.Equal(1, result);
            _mockContext.Verify(c => c.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPropagateCancellationToken()
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            _mockContext
                .Setup(c => c.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(0) // Simulate no changes saved
                .Verifiable();

            // Act
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Assert
            Assert.Equal(0, result);
            _mockContext.Verify(c => c.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldThrowExceptionIfContextThrows()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockContext
                .Setup(c => c.SaveChangesAsync(cancellationToken))
                .ThrowsAsync(new DbUpdateException())
                .Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() =>
                _unitOfWork.SaveChangesAsync(cancellationToken));

            _mockContext.Verify(c => c.SaveChangesAsync(cancellationToken), Times.Once);
        }
    }
}
