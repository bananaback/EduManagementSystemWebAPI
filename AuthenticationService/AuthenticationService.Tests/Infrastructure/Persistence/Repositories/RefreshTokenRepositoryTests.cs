using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AuthenticationService.Tests.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepositoryTests
    {
        private readonly DbContextOptions<AuthenticationDbContext> _options;
        private readonly AuthenticationDbContext _context;
        private readonly RefreshTokenRepository _repository;

        public RefreshTokenRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AuthenticationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new AuthenticationDbContext(_options);
            _repository = new RefreshTokenRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddRefreshTokenToDatabase()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var refreshToken = new RefreshToken(user, TokenValue.Create("new_token"));

            // Act
            await _repository.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByTokenAsync("new_token");
            result.Should().NotBeNull();
            result.Should().Be(refreshToken);
        }

        [Fact]
        public async Task Delete_ShouldRemoveRefreshTokenFromDatabase()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var refreshToken = new RefreshToken(user, TokenValue.Create("delete_token"));
            await _repository.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // Act
            _repository.Delete(refreshToken);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByTokenAsync("delete_token");
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAllByUserIdAsync_ShouldRemoveAllTokensForGivenUser()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var otherUser = new ApplicationUser
            (
                Username.Create("otheruser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );

            var token1 = new RefreshToken(user, TokenValue.Create("token1"));
            var token2 = new RefreshToken(user, TokenValue.Create("token2"));
            var otherUserToken = new RefreshToken(otherUser, TokenValue.Create("othertoken"));

            await _repository.AddAsync(token1);
            await _repository.AddAsync(token2);
            await _repository.AddAsync(otherUserToken);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAllByUserIdAsync(user.Id);
            await _context.SaveChangesAsync();

            // Assert
            var userTokens = _context.RefreshTokens.Where(rt => rt.UserId == user.Id).ToList();
            userTokens.Should().BeEmpty();

            var otherUserTokens = _context.RefreshTokens.Where(rt => rt.UserId == otherUser.Id).ToList();
            otherUserTokens.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetByTokenAsync_ShouldReturnCorrectRefreshToken()
        {
            // Arrange
            var user = new ApplicationUser
            (
                Username.Create("existinguser"),
                PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu"),
                RoleEnum.User
            );
            var refreshToken = new RefreshToken(user, TokenValue.Create("search_token"));
            await _repository.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByTokenAsync("search_token");

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(refreshToken);
        }
    }
}
