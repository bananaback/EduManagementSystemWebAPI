using AuthenticationService.Infrastructure.Services.PasswordHashers;
using FluentAssertions;

namespace AuthenticationService.Tests.Infrastructure.Services.PasswordHashers
{
    public class BcryptPasswordHasherTests
    {
        private readonly BcryptPasswordHasher _passwordHasher;

        public BcryptPasswordHasherTests()
        {
            _passwordHasher = new BcryptPasswordHasher();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            string rawPassword = "password123";

            // Act
            string hashedPassword = _passwordHasher.HashPassword(rawPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(rawPassword);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
        {
            // Arrange
            string rawPassword = "password123";
            string hashedPassword = _passwordHasher.HashPassword(rawPassword);

            // Act
            bool isValid = _passwordHasher.VerifyPassword(rawPassword, hashedPassword);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
        {
            // Arrange
            string rawPassword = "password123";
            string hashedPassword = _passwordHasher.HashPassword(rawPassword);
            string invalidPassword = "wrongpassword";

            // Act
            bool isValid = _passwordHasher.VerifyPassword(invalidPassword, hashedPassword);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void HashPassword_ShouldThrowException_WhenPasswordIsNull()
        {
            // Act
            Action act = () => _passwordHasher.HashPassword(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void VerifyPassword_ShouldThrowException_WhenPasswordIsNull()
        {
            // Arrange
            string hashedPassword = _passwordHasher.HashPassword("password123");

            // Act
            Action act = () => _passwordHasher.VerifyPassword(null!, hashedPassword);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void VerifyPassword_ShouldThrowException_WhenPasswordHashIsNull()
        {
            // Act
            Action act = () => _passwordHasher.VerifyPassword("password123", null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
