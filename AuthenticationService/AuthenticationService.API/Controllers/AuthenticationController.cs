using AuthenticationService.API.Requests;
using AuthenticationService.API.Responses;
using AuthenticationService.Application.Features.Login;
using AuthenticationService.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

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

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ErrorResponse("Username and password cannot be empty."));
            }    

            var command = new LoginUserCommand
            {
                UserName = request.UserName,
                Password = request.Password,
            };

            AuthenticatedUserResult res = await _mediator.Send(command, cancellationToken);

            if (res.IsSuccess)
            {
                return Ok(); //...
            }

            return Unauthorized();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
