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
    public class TokenValueTests
    {
        [Fact]
        public void Create_ShouldReturnTokenValue_WhenValueIsValid()
        {
            // Arrange
            var validTokenValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU";

            // Act
            var tokenValue = TokenValue.Create(validTokenValue);

            // Assert
            tokenValue.Value.Should().Be(validTokenValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_ShouldThrowInvalidTokenValueException_WhenValueIsInvalid(string invalidTokenValue)
        {
            // Act
            Action act = () => TokenValue.Create(invalidTokenValue);

            // Assert
            act.Should().Throw<InvalidTokenValueException>().WithMessage("Token value cannot be null or empty.");
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenTokenValuesAreSame()
        {
            // Arrange
            var tokenValue1 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");
            var tokenValue2 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");

            // Act & Assert
            tokenValue1.Should().Be(tokenValue2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenTokenValuesAreDifferent()
        {
            // Arrange
            var tokenValue1 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");
            var tokenValue2 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZzIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiYXVkIjpbImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiIsImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiIsImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiJdLCJuYmYiOjE3MzU1NDI3MzEsImV4cCI6MTczNTU0Mjc0NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEzNiJ9.G2VtciyE-doOEhfowPe5ju5jtY6FvZIF9JWLi7Edyzc");

            // Act & Assert
            tokenValue1.Should().NotBe(tokenValue2);
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_WhenPasswordHashsAreSame()
        {
            // Arrange
            var tokenValue1 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");
            var tokenValue2 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");

            // Act
            var hashCode1 = tokenValue1.GetHashCode();
            var hashCode2 = tokenValue2.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_WhenPasswordHashsAreDifferent()
        {
            // Arrange
            var tokenValue1 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2IiwiaHR0cHM6Ly9sb2NhaG9zdDo3MTM2Il0sIm5iZiI6MTczNTU0MjczMSwiZXhwIjoxNzM1NTQyNzQ2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTM2In0.uDkLxgiZwrk6sqOSUW0JX5aJimJ9p3NgpkFdAVPMwIU");
            var tokenValue2 = TokenValue.Create("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjhiNmNiODAwLWE5NGItNGEzYy05ZWFlLTRlNzk5YjE2NjhkOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJkZWJ1ZzIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiYXVkIjpbImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiIsImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiIsImh0dHBzOi8vbG9jYWhvc3Q6NzEzNiJdLCJuYmYiOjE3MzU1NDI3MzEsImV4cCI6MTczNTU0Mjc0NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzEzNiJ9.G2VtciyE-doOEhfowPe5ju5jtY6FvZIF9JWLi7Edyzc");

            // Act
            var hashCode1 = tokenValue1.GetHashCode();
            var hashCode2 = tokenValue2.GetHashCode();

            // Assert
            hashCode1.Should().NotBe(hashCode2);
        }
    }
}
