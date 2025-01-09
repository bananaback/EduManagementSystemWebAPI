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
    public class PersonNameTests
    {
        [Theory]
        [InlineData("John", "Doe")]
        [InlineData("Jane", "Doe")]
        [InlineData("John", "Smith")]
        [InlineData("Jane", "Smith")]
        public void ValidValue_ShouldBePassed_WhenInitialized(string firstName, string lastName)
        {
            // Arrange
            var personName = new PersonName(firstName, lastName);

            // Act & Assert
            Assert.Equal($"{firstName} {lastName}", personName.FullName);
        }

        [Theory]
        [InlineData(null, "Doe")]
        [InlineData("", "Doe")]
        [InlineData("John", null)]
        [InlineData("John", "")]
        [InlineData("", "")]
        [InlineData("Ann", "#das")]
        [InlineData("Adnn2", "da2#")]
        public void PersonName_ShouldThrowException_WhenInvalidValuePassed(string firstName, string lastName)
        {
            // Act
            Action act = () => new PersonName(firstName, lastName);

            // Assert
            act.Should().Throw<InvalidPersonNameException>();
        }

        [Fact]
        public void PersonName_ShouldNotBePassed_WhenExceeds50Characters()
        {
            // Arrange
            var longFirstName = "This is a very long first name";
            var longLastName = "This is a very long last name";

            // Act
            Action act = () => new PersonName(longFirstName, longLastName);

            // Assert
            act.Should().Throw<InvalidPersonNameException>();
        }

        [Fact]
        public void PersonName_ShouldBeEqual_WhenPropertiesAreTheSame()
        {
            // Arrange
            var personName1 = new PersonName("John", "Doe");
            var personName2 = new PersonName("John", "Doe");

            // Act & Assert
            personName1.Should().Be(personName2);
        }

        [Fact]
        public void PersonName_ShouldNotBeEqual_WhenPropertiesAreDifferent()
        {
            // Arrange
            var personName1 = new PersonName("John", "Doe");
            var personName2 = new PersonName("Jane", "Doe");

            // Act & Assert
            personName1.Should().NotBe(personName2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenPersonNamesAreSame()
        {
            // Arrange
            var personName1 = new PersonName("John", "Doe");
            var personName2 = new PersonName("John", "Doe");

            // Act
            var hashCode1 = personName1.GetHashCode();
            var hashCode2 = personName2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldNotBeSame_WhenPersonNamesAreDifferent()
        {
            // Arrange
            var personName1 = new PersonName("John", "Doe");
            var personName2 = new PersonName("Jane", "Doe");

            // Act
            var hashCode1 = personName1.GetHashCode();
            var hashCode2 = personName2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
