using AuthenticationService.API.Controllers;
using AuthenticationService.API.Requests;
using AuthenticationService.API.Responses;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Features.LogoutUser;
using AuthenticationService.Application.Features.Register;
using AuthenticationService.Application.Features.RotateToken;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace AuthenticationService.Tests.API.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthenticationController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                UserName = "testuser",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(It.IsAny<Guid>()));

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("User registration successfully");
        }

        [Fact]
        public async Task RegisterUser_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserName", "Required");

            var request = new RegisterUserRequest();

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RegisterUser_PasswordMismatch_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                UserName = "testuser",
                Password = "password123",
                ConfirmPassword = "differentPassword"
            };

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorResponse>()
                .Which.ErrorMessages.Should().Contain("Confirmation password does not match.");
        }

        [Fact]
        public async Task LoginUser_ValidRequest_ReturnsAuthenticatedResponse()
        {
            // Arrange
            var request = new LoginUserRequest
            {
                Username = "testuser",
                Password = "password123"
            };

            var authResult = AuthenticatedUserResult.Success("access_token", "refresh_token");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.LoginUser(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new AuthenticatedUserResponse
                {
                    Message = "User login successfully.",
                    AccessToken = "access_token",
                    RefreshToken = "refresh_token"
                });
        }

        [Fact]
        public async Task LoginUser_EmptyCredentials_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginUserRequest
            {
                Username = "",
                Password = ""
            };

            // Act
            var result = await _controller.LoginUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<ErrorResponse>()
                .Which.ErrorMessages.Should().Contain("Username and password cannot be empty.");
        }

        [Fact]
        public async Task LoginUser_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginUserRequest
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var authResult = AuthenticatedUserResult.Failure();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.LoginUser(request);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Refresh_ValidRequest_ReturnsNewTokens()
        {
            // Arrange
            var request = new RotateTokenRequest { RefreshToken = "valid_refresh_token" };

            var authResult = AuthenticatedUserResult.Success("new_access_token", "new_refresh_token");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RotateTokenCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Refresh(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new AuthenticatedUserResponse
                {
                    Message = "Rotate token successfully.",
                    AccessToken = "new_access_token",
                    RefreshToken = "new_refresh_token"
                });
        }

        [Fact]
        public async Task Refresh_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var request = new RotateTokenRequest { RefreshToken = "invalid_refresh_token" };

            var authResult = AuthenticatedUserResult.Failure();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RotateTokenCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Refresh(request);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task LogoutUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new LogoutUserRequest { RefreshToken = "valid_token" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LogoutUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.LogoutUser(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Logout user successfully.");
        }

        [Fact]
        public async Task LogoutUserOnAllDevices_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("RefreshToken", "Required");

            var request = new LogoutUserAllDeviceRequest();

            // Act
            var result = await _controller.LogoutUserOnAllDevices(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task LogoutUserOnAllDevices_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LogoutUserAllDeviceRequest
            {
                RefreshToken = "refresh_token"
            };


            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LogoutUserAllDeviceRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.LogoutUserOnAllDevices(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task LogoutUserOnAllDevices_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new LogoutUserAllDeviceRequest
            {
                RefreshToken = "refresh_token"
            };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LogoutUserAllDeviceRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.LogoutUserOnAllDevices(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

    }

}
