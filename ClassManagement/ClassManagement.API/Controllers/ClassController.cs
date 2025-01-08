using ClassManagement.API.Requests;
using ClassManagement.API.Responses;
using ClassManagement.Application.Features.Classes.Commands.Create;
using ClassManagement.Application.Features.Classes.Commands.Delete;
using ClassManagement.Application.Features.Classes.Commands.Edit;
using ClassManagement.Application.Features.Classes.Commands.EnrollStudent;
using ClassManagement.Application.Features.Classes.Queries.GetAll;
using ClassManagement.Application.Features.Classes.Queries.GetById;
using ClassManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassManagement.API.Controllers
{
    [ApiController]
    [Route("/api/classes")]
    [Authorize(Roles = "Admin,User")]
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

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new CreateClassCommand
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                MaxCapacity = request.MaxCapacity
            };

            var classId = await _mediator.Send(command, cancellationToken);

            var readCommand = new GetClassByIdCommand
            {
                Id = classId
            };

            var classReadDto = await _mediator.Send(readCommand, cancellationToken);

            return CreatedAtRoute(nameof(GetClassById), new { id = classId }, classReadDto);
        }

        [AllowAnonymous]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditClassById(Guid id, [FromBody] EditClassRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new EditClassCommand
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                MaxCapacity = request.MaxCapacity
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllClasses(
            Guid? id = null,
            string? name = null,
            string? description = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            ClassStatus? status = null,
            byte? maxCapacity = null,
            int? pageNumber = 1,
            int? itemsPerPage = 5,
            CancellationToken cancellationToken = default)
        {
            var command = new GetAllClassesCommand
            {
                Id = id,
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                MaxCapacity = maxCapacity,
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage
            };

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

        [HttpPost("{id:guid}/enrollments")]
        public async Task<IActionResult> EnrollStudentInClass(Guid id, EnrollStudentInClassRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new EnrollStudentCommand
            {
                ClassId = id,
                StudentId = request.StudentId,
            };

            var res = await _mediator.Send(command, cancellationToken);

            if (res)
            {
                return Ok($"Enroll student with id {request.StudentId} into class with id {id} successfully.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to enroll student into this class");
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
