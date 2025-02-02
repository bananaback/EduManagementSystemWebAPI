﻿using AuthenticationService.API.Requests;
using AuthenticationService.API.Responses;
using AuthenticationService.API.Validators;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Features.LogoutUser;
using AuthenticationService.Application.Features.Register;
using AuthenticationService.Application.Features.RotateToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly RequestValidator _requestValidator;
        public AuthenticationController(IMediator mediator, RequestValidator requestValidator)
        {
            _mediator = mediator;
            _requestValidator = requestValidator ?? throw new ArgumentException(nameof(requestValidator));  
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            request.Initialize(_requestValidator);

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Confirmation password does not match."));
            }

            var command = new RegisterUserCommand
            {
                Username = request.UserName,
                Password = request.Password,
            };

            await _mediator.Send(command, cancellationToken);

            return Ok("User registration successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            request.Initialize(_requestValidator);

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ErrorResponse("Username and password cannot be empty."));
            }    

            var command = new LoginUserCommand
            {
                UserName = request.Username,
                Password = request.Password,
            };

            AuthenticatedUserResult res = await _mediator.Send(command, cancellationToken);

            if (res.IsSuccess)
            {
                return Ok(new AuthenticatedUserResponse
                {
                    Message = "User login successfully.",
                    AccessToken = res.AccessToken!.Value,
                    RefreshToken = res.RefreshToken!.Value
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RotateTokenRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new RotateTokenCommand(request.RefreshToken);

            var res = await _mediator.Send(command, cancellationToken);

            if (res.IsSuccess)
            {
                return Ok(new AuthenticatedUserResponse
                {
                    Message = "Rotate token successfully.",
                    AccessToken = res.AccessToken!.Value,
                    RefreshToken = res.RefreshToken!.Value
                });
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new LogoutUserCommand(request.RefreshToken);

            var res = await _mediator.Send(command, cancellationToken);

            if (res)
            {
                return Ok("Logout user successfully.");
            } else
            {
                return Ok("Failed to logout user. Plase try again.");
            }
        }

        [Authorize]
        [HttpPost("logout/all")]
        public async Task<IActionResult> LogoutUserOnAllDevices([FromBody] LogoutUserAllDeviceRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new LogoutUserAllDevicesCommand(request.RefreshToken);

            var res = await _mediator.Send(command, cancellationToken);

            if (res)
            {
                return Ok("Logout user on all devices successfully.");
            }
            else
            {
                return Unauthorized("Failed to logout user on all devices. Plase try again.");
            }
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
