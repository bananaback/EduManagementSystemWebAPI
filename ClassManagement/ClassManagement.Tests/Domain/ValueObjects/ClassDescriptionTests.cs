using ClassManagement.Domain.Exceptions;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;


namespace ClassManagement.Tests.Domain.ValueObjects
{
    public class ClassDescriptionTests
    {
        [Fact]
        public void ClassDescription_ShouldHaveCorrectProperties_WhenInitialized()
        {
            // Arrange
            var classDescription = new ClassDescription("This is a class description.");

            // Act & Assert
            classDescription.Value.Should().Be("This is a class description.");

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("asdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdjasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyujasdqwertyuj")]
        public void ClassDescription_ShouldThrowExceptionForInvalidProperties_WhenInitialized(string descriptionValue)
        {
            // Act
            Action act = () => new ClassDescription(descriptionValue);

            // Assert
            act.Should().Throw<InvalidClassDescriptionException>();
        }

        [Fact]
        public void ClassDescription_ShouldBeEqual_WhenPropertiesAreTheSame()
        {
            // Arrange
            var classDescription1 = new ClassDescription("This is a class description.");
            var classDescription2 = new ClassDescription("This is a class description.");

            // Act & Assert
            classDescription1.Should().Be(classDescription2);
        }

        [Fact]
        public void ClassDescription_ShouldNotBeEqual_WhenPropertiesAreTheDifferent()
        {
            // Arrange
            var classDescription1 = new ClassDescription("This is a class description.");
            var classDescription2 = new ClassDescription("This is a class description.2");

            // Act & Assert
            classDescription1.Should().NotBe(classDescription2);
        }

        [Fact]
        public void ClassDescriptionSpaces_ShouldBeTrimmed_WhenInitialized()
        {
            // Arrange
            var value = "  s pa  ce s  ";
            var expected = "s pa  ce s";

            // Act
            var description = new ClassDescription(value);

            // Assert
            description.Value.Should().Be(expected);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenClassDescriptionsAreSame()
        {
            // Arrange
            var classDescription1 = new ClassDescription("This is a class description");
            var classDescription2 = new ClassDescription("This is a class description");

            // Act
            var hashCode1 = classDescription1.GetHashCode();
            var hashCode2 = classDescription1.GetHashCode();
        
            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenClassDescriptionsAreDifferent()
        {
            // Arrange
            var classDescription1 = new ClassDescription("This is a class description");
            var classDescription2 = new ClassDescription("This is something different");

            // Act
            var hashCode1 = classDescription1.GetHashCode();
            var hashCode2 = classDescription2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}