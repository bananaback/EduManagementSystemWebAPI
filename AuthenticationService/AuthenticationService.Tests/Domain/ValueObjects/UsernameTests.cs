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
    public class UsernameTests
    {
        [Fact]
        public void Create_ShouldReturnUsername_WhenValueIsValid()
        {
            // Arrange
            var validUsername = "bananaback";

            // Act
            var username = Username.Create(validUsername);

            // Assert
            username.Value.Should().Be(validUsername);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Create_ShouldThrowInvalidUsernameException_WhenValueIsEmpty(string invalidUsername)
        {
            // Act
            Action act = () => Username.Create(invalidUsername);

            // Assert
            act.Should().Throw<InvalidUsernameException>().WithMessage("Username cannot be null or empty.");
        }

        [Theory]
        [InlineData("ban")]
        [InlineData("thisistoolongusernametobeallowed")]
        [InlineData("2startwithnumber.")]
        public void Create_ShouldThrowInvalidUsernameException_WhenValueIsInvalid(string invalidUsername)
        {
            // Act
            Action act = () => Username.Create(invalidUsername);

            // Assert
            act.Should().Throw<InvalidUsernameException>().WithMessage("Username must have must contains only alphanumeric, underscore or dot. It starts and ends with an alphanumeric character. Length in range 4-30 characters.");
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenUsernamesAreSame()
        {
            // Arrange
            var username1 = Username.Create("bananaback");
            var username2 = Username.Create("bananaback");
        
            // Act & assert
            username1.Should().Be(username2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenUsernamesAreDifferent()
        {
            // Arrange
            var username1 = Username.Create("banana");
            var username2 = Username.Create("apple");
            username1.Should().NotBe(username2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenUsernamesAreSame()
        {
            // Arrange 
            var username1 = Username.Create("bananaback");
            var username2 = Username.Create("bananaback");

            // Act
            var hashCode1 = username1.GetHashCode();
            var hashCode2 = username2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2); 
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenUsernamesAreDifferent()
        {
            // Arrange 
            var username1 = Username.Create("bananaback");
            var username2 = Username.Create("appleback");

            // Act
            var hashCode1 = username1.GetHashCode();
            var hashCode2 = username2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
