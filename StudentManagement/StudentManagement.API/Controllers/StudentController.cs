using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Requests;
using StudentManagement.API.Responses;
using StudentManagement.Application.Features.Students.Commands.Create;
using StudentManagement.Application.Features.Students.Commands.Delete;
using StudentManagement.Application.Features.Students.Commands.Edit;
using StudentManagement.Application.Features.Students.Queries.GetAll;
using StudentManagement.Application.Features.Students.Queries.GetById;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("/api/students")]
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

        [HttpPost]
        public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateStudentCommand
            {
                Name = request.Name,
                Email = request.Email,
                EnrollmentDate = request.EnrollmentDate
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
                Id = id,
                Name = request.Name,
                Email = request.Email,
                EnrollmentDate = request.EnrollmentDate,
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
