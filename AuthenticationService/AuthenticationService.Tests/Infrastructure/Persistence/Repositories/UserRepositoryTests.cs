using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Tests.Infrastructure.Persistence.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<AuthenticationDbContext> _options;
        private readonly AuthenticationDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AuthenticationDbContext>()
                .UseInMemoryDatabase("TestDatabase_UserRepository")
                .Options;

            _context = new AuthenticationDbContext(_options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("newuser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            // Act
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByUsernameAsync("newuser");
            result.Should().NotBeNull();
            result.Should().Be(user);
        }

        [Fact]
        public async Task Delete_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("user_to_delete"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            _repository.Delete(user);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByUsernameAsync("user_to_delete");
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var user1 = new ApplicationUser
            (
                Username.Create("user1"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var user2 = new ApplicationUser
            (
                Username.Create("user2"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.Admin
            );


            await _repository.AddAsync(user1);
            await _repository.AddAsync(user2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAll();

            // Assert
            result.Should().Contain(user1);
            result.Should().Contain(user2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("user_by_id"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(user);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("specific_username"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("specific_username");

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(user);
        }
    }
}
