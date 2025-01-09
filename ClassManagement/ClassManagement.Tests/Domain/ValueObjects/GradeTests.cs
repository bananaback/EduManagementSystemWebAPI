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
    public class GradeTests
    {
        [Theory]
        [InlineData("A+")]
        [InlineData("A")]
        [InlineData("A-")]
        [InlineData("B+")]
        [InlineData("B")]
        [InlineData("B-")]
        [InlineData("C+")]
        [InlineData("C")]
        [InlineData("C-")]
        [InlineData("D+")]
        [InlineData("D")]
        [InlineData("D-")]
        [InlineData("F")]
        [InlineData("N/A")]
        public void ValidGrade_ShouldBePassed_WhenInitialized(string value)
        {
            // Arrange
            var grade = new Grade(value);

            // Act & Assert
            Assert.Equal(value, grade.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("A++")]
        [InlineData("A--")]
        [InlineData("B++")]
        [InlineData("B--")]
        [InlineData("C++")]
        [InlineData("C--")]
        [InlineData("D++")]
        [InlineData("D--")]
        [InlineData("F+")]
        [InlineData("F-")]
        [InlineData("N/A+")]
        [InlineData("N/A-")]
        public void Grade_ShouldThrowException_WhenInvalidValuePassed(string value)
        {
            // Act
            Action act = () => new Grade(value);

            // Assert
            act.Should().Throw<InvalidGradeException>();
        }

        [Fact]
        public void Grade_ShouldBeEqual_WhenPropertiesAreTheSame()
        {
            // Arrange
            var grade1 = new Grade("A");
            var grade2 = new Grade("A");

            // Act & Assert
            grade1.Should().Be(grade2);
        }

        [Fact]
        public void Grade_ShouldNotBeEqual_WhenPropertiesAreDifferent()
        {
            // Arrange
            var grade1 = new Grade("A");
            var grade2 = new Grade("B");

            // Act & Assert
            grade1.Should().NotBe(grade2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenGradesAreSame()
        {
            // Arrange
            var grade1 = new Grade("A");
            var grade2 = new Grade("A");

            // Act
            var hashCode1 = grade1.GetHashCode();
            var hashCode2 = grade2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldNotBeSame_WhenGradesAreDifferent()
        {
            // Arrange
            var grade1 = new Grade("A");
            var grade2 = new Grade("B");

            // Act
            var hashCode1 = grade1.GetHashCode();
            var hashCode2 = grade2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
