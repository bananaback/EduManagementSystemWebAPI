using ClassManagement.API.Requests;
using ClassManagement.API.Responses;
using ClassManagement.Application.Features.Classes.Commands.Create;
using ClassManagement.Application.Features.Classes.Commands.Delete;
using ClassManagement.Application.Features.Classes.Commands.Edit;
using ClassManagement.Application.Features.Classes.Queries.GetAll;
using ClassManagement.Application.Features.Classes.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagement.API.Controllers
{
    [ApiController]
    [Route("/api/classes")]
    public class ClassController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok("Endpoint reached!");
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new CreateClassCommand
            {
                ClassName = request.ClassName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };

            var classId = await _mediator.Send(command, cancellationToken);

            var readCommand = new GetClassByIdCommand
            {
                Id = classId
            };

            var classReadDto = await _mediator.Send(readCommand, cancellationToken);

            return CreatedAtRoute(nameof(GetClassById), new { id = classId }, classReadDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditClassById(Guid id, [FromBody] EditClassRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new EditClassCommand
            {
                Id = id,
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };

            var updatedClassId = await _mediator.Send(command, cancellationToken);

            return Ok($"Edit class with id {id} successfully.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClassById(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new DeleteClassCommand { Id = id };

            var deletedClassId = await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses(CancellationToken cancellationToken = default)
        {
            var command = new GetAllClassesCommand();

            var classReadDtos = await _mediator.Send(command, cancellationToken);

            return Ok(classReadDtos);
        }

        [HttpGet("{id:guid}", Name = nameof(GetClassById))]
        public async Task<IActionResult> GetClassById(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            var command = new GetClassByIdCommand { Id = id };
            
            var classReadDto = await _mediator.Send(command, cancellationToken);
            
            return Ok(classReadDto);
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
