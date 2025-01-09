using ClassManagement.Domain.Exceptions;
using ClassManagement.Domain.ValueObjects;
using FluentAssertions;

namespace ClassManagement.Tests.Domain.ValueObjects
{
    public class AddressTests
    {
        [Fact]
        public void Address_Should_Have_Correct_Properties()
        {
            // Arrange
            var houseNumber = "14/38";
            var street = "Duong Lang Tang Phu";
            var ward = "Tang Nhon Phu A";
            var district = "Quan 9";
            var city = "TP Ho Chi Minh";
            var address = new Address(houseNumber, street, ward, district, city);

            // Act & Assert
            address.HouseNumber.Should().Be(houseNumber);
            address.Street.Should().Be(street);
            address.Ward.Should().Be(ward);
            address.District.Should().Be(district);
            address.City.Should().Be(city);
        }

        [Theory]
        [InlineData("", "", "", "", "")]
        [InlineData(null, null, null, null, null)]
        [InlineData("14/38dasdasdasdasdasdadadadadadadadasdasdadasdadasddasdasdasdasdasdadadadadadadadasdasdadasdadasd", "", "", "", "")]
        public void Address_Should_Throw_Exception_For_Invalid_Properties(
            string houseNumber,
            string street,
            string ward,
            string district,
            string city)
        {
            // Act
            Action act = () => new Address(houseNumber, street, ward, district, city);

            // Assert
            act.Should().Throw<InvalidAddressException>();
        }

        [Fact]
        public void Address_Should_Be_Equal_When_Properties_Are_The_Same()
        {
            // Arrange
            var address1 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");
            var address2 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");

            // Act & Assert
            address1.Should().Be(address2);
        }

        [Fact]
        public void Address_Should_Not_Be_Equal_When_Properties_Are_Different()
        {
            // Arrange
            var address1 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");
            var address2 = new Address("18", "Bui Thi Xuan", "An Lao", "An Lao", "Tinh Binh Dinh");

            // Act & Assert
            address1.Should().NotBe(address2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenAddressesAreSame()
        {
            // Arrange
            var address1 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");
            var address2 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");

            // Act
            var hashCode1 = address1.GetHashCode();
            var hashCode2 = address2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenAddressesAreDifferent()
        {
            // Arrange
            var address1 = new Address("14/38", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");
            var address2 = new Address("14/39", "Duong Lang Tang Phu", "Tang Nhon Phu A", "Quan 9", "TP Ho Chi Minh");

            // Act
            var hashCode1 = address1.GetHashCode();
            var hashCode2 = address2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
