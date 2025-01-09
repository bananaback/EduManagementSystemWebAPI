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
    public class ClassNameTests
    {
        [Fact]
        public void ClassName_ShouldHaveValidProperties_WhenInitialized()
        {
            // Arrange
            var className = new ClassName("Math");

            // Act
            var result = className.Value;

            // Assert
            Assert.Equal("Math", result);
        }

        [Theory]
        [InlineData("Math")]
        [InlineData("English")]
        [InlineData("History")]
        [InlineData("Science")]
        [InlineData("Information Technology")]
        public void ValidValue_ShouldBePassed_WhenInitialized(string value)
        {
            // Arrange
            var className = new ClassName(value);

            // Act & Assert
            Assert.Equal(value, className.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Math 2")]
        [InlineData("Bio-logy")]
        [InlineData("Chemistry!")]
        [InlineData("This is a very long class name that is more than 100 characters longggggggggggggggggggggggggggggggggggggggggggggggggggggggg")]
        public void ClassName_ShouldThrowException_WhenInvalidValuePassed(string value)
        {
            // Act
            Action act = () => new ClassName(value);

            // Assert
            act.Should().Throw<InvalidClassNameException>();
        }

        [Fact]
        public void ClassName_ShouldBeEqual_WhenPropertiesAreTheSame()
        {
            // Arrange
            var className1 = new ClassName("Math");
            var className2 = new ClassName("Math");

            // Act & Assert
            className1.Should().Be(className2);
        }

        [Fact]
        public void ClassName_ShouldNotBeEqual_WhenPropertiesAreDifferent()
        {
            // Arrange
            var className1 = new ClassName("Math");
            var className2 = new ClassName("English");

            // Act & Assert
            className1.Should().NotBe(className2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenClassNamesAreSame()
        {
            // Arrange
            var className1 = new ClassName("Math");
            var className2 = new ClassName("Math");

            // Act
            var hashCode1 = className1.GetHashCode();
            var hashCode2 = className2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldNotBeSame_WhenClassNamesAreDifferent()
        {
            // Arrange
            var className1 = new ClassName("Math");
            var className2 = new ClassName("IT");

            // Act
            var hashCode1 = className1.GetHashCode();
            var hashCode2 = className2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
