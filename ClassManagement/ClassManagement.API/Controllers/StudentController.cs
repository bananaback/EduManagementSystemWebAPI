using ClassManagement.API.Requests;
using ClassManagement.API.Responses;
using ClassManagement.Application.Features.Classes.Commands.Create;
using ClassManagement.Application.Features.Classes.Commands.Delete;
using ClassManagement.Application.Features.Classes.Commands.Edit;
using ClassManagement.Application.Features.Classes.Queries.GetAll;
using ClassManagement.Application.Features.Classes.Queries.GetById;
using ClassManagement.Application.Features.Students.Commands.Create;
using ClassManagement.Application.Features.Students.Commands.Delete;
using ClassManagement.Application.Features.Students.Commands.Edit;
using ClassManagement.Application.Features.Students.Queries.GetAll;
using ClassManagement.Application.Features.Students.Queries.GetById;
using ClassManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ClassManagement.API.Controllers
{
    [ApiController]
    [Route("/api/students")]
    [Authorize(Roles = "Admin,User")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok("Endpoint reached!");
        }

        [HttpGet]

        public async Task<IActionResult> GetAllStudentsAsync(CancellationToken cancellationToken = default)
        {
            var command = new GetAllStudentsCommand();

            var students = await _mediator.Send(command, cancellationToken);

            return Ok(students);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            var command = new CreateStudentCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                EnrollmentDate = request.EnrollmentDate,
                Address = new Address(request.Address.HouseNumber, request.Address.Street, request.Address.Ward, request.Address.District, request.Address.City),
                ExposePrivateInfo = request.ExposePrivateInfo
            };

            var createdStudentId = await _mediator.Send(command, cancellationToken);

            var readCommand = new GetStudentByIdCommand
            {
                Id = createdStudentId
            };

            var studentReadDto = await _mediator.Send(readCommand, cancellationToken);

            return CreatedAtRoute(nameof(GetStudentById), new { id = createdStudentId }, studentReadDto);
        }

        [HttpGet("{id:guid}", Name = nameof(GetStudentById))]
        public async Task<IActionResult> GetStudentById(Guid id, CancellationToken cancellationToken = default)
        {
            var command = new GetStudentByIdCommand
            {
                Id = id
            };

            var studentReadDto = await _mediator.Send(command, cancellationToken);

            return Ok(studentReadDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditStudentById(Guid id, [FromBody] EditStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new EditStudentCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                EnrollmentDate = request.EnrollmentDate,
                Address = (request.Address != null) ? new Address(request.Address.HouseNumber, request.Address.Street, request.Address.Ward, request.Address.District, request.Address.City) : null,
                ExposePrivateInfo = request.ExposePrivateInfo
            };

            var updatedStudentId = await _mediator.Send(command, cancellationToken);

            return Ok($"Edit student with id {id} successfully.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudentById(Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteStudentCommand
            {
                Id = id
            };

            var deletedStudentId = await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
