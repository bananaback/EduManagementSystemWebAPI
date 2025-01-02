using AuthenticationService.Domain.Exceptions;
using AuthenticationService.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Tests.Domain.ValueObjects
{
    public class PasswordHashTests
    {
        [Fact]
        public void Create_ShouldReturnHashPassword_WhenValueIsValid()
        {
            // Arrange
            var validPasswordHash = "$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu";

            // Act
            var passwordHash = PasswordHash.Create(validPasswordHash);

            // Assert
            passwordHash.Value.Should().Be(validPasswordHash);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Create_ShouldThrowInvalidHashPasswordException_WhenValueIsNullOrEmpty(string invalidPasswordHash)
        {
            // Act
            Action act = () => PasswordHash.Create(invalidPasswordHash);

            // Assert
            act.Should().Throw<InvalidPasswordHashException>().WithMessage("Password hash cannot be null or empty.");
        }

        [Theory]
        [InlineData("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7h")]
        [InlineData("s2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu")]
        public void Create_ShouldThrowInvalidHashPasswordException_WhenValueIsNotValidBcryptFormat(string invalidPasswordHash)
        {
            // Act
            Action act = () => PasswordHash.Create(invalidPasswordHash);

            // Assert
            act.Should().Throw<InvalidPasswordHashException>().WithMessage("Invalid bcrypt format.");
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenPasswordHashsAreSame()
        {
            // Arrange
            var passwordHash1 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu");
            var passwordHash2 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu");

            // Act & Assert
            passwordHash1.Should().Be(passwordHash2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenPasswordHashsAreDifferent()
        {
            // Arrange
            var passwordHash1 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hd");
            var passwordHash2 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hw");

            // Act & Assert
            passwordHash1.Should().NotBe(passwordHash2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenPasswordHashsAreSame()
        {
            // Arrange
            var passwordHash1 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu");
            var passwordHash2 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hu");

            // Act
            var hashCode1 = passwordHash1.GetHashCode();
            var hashCode2 = passwordHash2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenPasswordHashsAreDifferent()
        {
            // Arrange
            var passwordHash1 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hd");
            var passwordHash2 = PasswordHash.Create("$2a$11$ZklnbtqgvjMSnhEGHHHTzeAIhknfLW9L1IRPOtzzLCo0Wd7B7G7hw");

            // Act
            var hashCode1 = passwordHash1.GetHashCode();
            var hashCode2 = passwordHash2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
