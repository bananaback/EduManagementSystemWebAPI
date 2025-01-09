using ClassManagement.Domain.Exceptions;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Tests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("bananaback@gmail.com")]
        [InlineData("banana@dev.com")]
        [InlineData("vttabcde@sss.com")]
        public void ValidEmail_ShouldBePassed_WhenInitialized(string value)
        {
            // Arrange
            var email = new Email(value);

            // Act & Assert
            Assert.Equal(value, email.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("bananabackgmail.com")]
        [InlineData("banana@devcom")]
        [InlineData("vttabcde@ssscom")]
        [InlineData("vttabcde@sss.")]
        [InlineData("vttabcde@sss")]
        [InlineData("vttabcde@.com")]
        public void Email_ShouldThrowException_WhenInvalidValuePassed(string value)
        {
            // Act
            Action act = () => new Email(value);

            // Assert
            act.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void Email_ShouldBeEqual_WhenPropertiesAreTheSame()
        {
            // Arrange
            var email1 = new Email("apple@gmail.com");
            var email2 = new Email("apple@gmail.com");

            // Act & Assert
            email1.Should().Be(email2);
        }

        [Theory]
        [InlineData("apple@gmail.com", "apPle@gmail.com")]
        [InlineData("apple@gmail.com", "mango@gmail.com")]
        public void Email_ShouldNotBeEqual_WhenPropertiesAreDifferent(string value1, string value2)
        {
            // Arrange
            var email1 = new Email(value1);
            var email2 = new Email(value2);

            // Act & Assert
            email1.Should().NotBe(email2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenEmailsAreSame()
        {
            // Arrange
            var email1 = new Email("banana@gmail.com");
            var email2 = new Email("banana@gmail.com");

            // Act
            var hashCode1 = email1.GetHashCode();
            var hashCode2 = email2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenEmailsAreDifferent()
        {
            // Arrange
            var email1 = new Email("it@it.com");
            var email2 = new Email("it@ti.com");

            // Act
            var hashCode1 = email1.GetHashCode();
            var hashCode2 = email2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}