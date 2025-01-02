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
    public class PasswordTests
    {
        [Fact]
        public void Create_ShouldReturnPassword_WhenValueIsValid()
        {
            // Arrange
            var validPassword = "StrongPassword100%";

            // Act
            var password = Password.Create(validPassword);

            // Assert
            password.Value.Should().Be(validPassword);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Create_ShouldThrowInvalidPasswordException_WhenValueIsNullOrEmpty(string invalidPassword)
        {
            // Act
            Action act = () => Password.Create(invalidPassword);

            // Assert
            act.Should().Throw<InvalidPasswordException>().WithMessage("Password cannot be null or empty.");
        }

        [Theory]
        [InlineData("weak")]
        [InlineData("nouppercase1!")]
        [InlineData("NOLOWERCASE1!")]
        [InlineData("NoSpecialChar1")]
        [InlineData("NoNumber!")]
        [InlineData("short1!")]
        [InlineData("thispasswordiswaytoolongtobevalidandshouldfailvalidation1!")]
        public void Create_ShouldThrowInvalidPasswordException_WhenPasswordStrengthIsInvalid(string invalidPassword)
        {
            // Act
            Action act = () => Password.Create(invalidPassword);

            //Assert
            act.Should().Throw<InvalidPasswordException>().WithMessage("Password must contains atleast one uppercase, one lowercase, one digit and one special character with a length in range 8-64, no space is allowed.");
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenPasswordsAreSame()
        {
            // Arrange
            var password1 = Password.Create("StrongPass1!");
            var password2 = Password.Create("StrongPass1!");

            // Act & Assert
            password1.Should().Be(password2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenPasswordsAreDifferent()
        {
            // Arrange
            var password1 = Password.Create("StrongPass1!");
            var password2 = Password.Create("StrongPass2!");

            // Act & Assert
            password1.Should().NotBe(password2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenPasswordsAreSame()
        {
            // Arrange
            var password1 = Password.Create("StrongPass1!");
            var password2 = Password.Create("StrongPass1!");

            // Act
            var hashCode1 = password1.GetHashCode();
            var hashCode2 = password2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenPasswordsAreDifferent()
        {
            // Arrange
            var password1 = Password.Create("StrongPass1!");
            var password2 = Password.Create("AnotherPass1!");

            // Act
            var hashCode1 = password1.GetHashCode();
            var hashCode2 = password2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
