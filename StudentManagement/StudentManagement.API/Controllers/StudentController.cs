using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Requests;
using StudentManagement.API.Responses;
using StudentManagement.Application.Features.Students.Commands.Create;
using StudentManagement.Application.Features.Students.Commands.Delete;
using StudentManagement.Application.Features.Students.Commands.Edit;
using StudentManagement.Application.Features.Students.Queries.GetAll;
using StudentManagement.Application.Features.Students.Queries.GetById;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.API.Controllers
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

        [AllowAnonymous]
        [HttpGet]

        public async Task<IActionResult> GetAllStudentsAsync(
            Guid? id = null,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            GenderEnum? gender = null,
            DateOnly? dateOfBirthBefore = null,
            DateOnly? dateOfBirthAfter = null,
            DateOnly? enrollmentDateBefore = null,
            DateOnly? enrollmentDateAfter = null,
            string? houseNumber = null,
            string? street = null,
            string? ward = null,
            string? district = null,
            string? city = null,
            int? pageNumber = 1,
            int? itemsPerPage = 5,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new GetAllStudentsCommand
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Gender = gender,
                DateOfBirthBefore = dateOfBirthBefore,
                DateOfBirthAfter = dateOfBirthAfter,
                EnrollmentDateBefore = enrollmentDateBefore,
                EnrollmentDateAfter = enrollmentDateAfter,
                HouseNumber = houseNumber,
                Street = street,
                Ward = ward,
                District = district,
                City = city,
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage
            };

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

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
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

        [AllowAnonymous]
        [HttpGet("{id:guid}", Name = nameof(GetStudentById))]
        public async Task<IActionResult> GetStudentById(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var command = new GetStudentByIdCommand
            {
                Id = id
            };

            var studentReadDto = await _mediator.Send(command, cancellationToken);

            return Ok(studentReadDto);
        }

        [AllowAnonymous]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditStudentById(Guid id, [FromBody] EditStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new EditStudentCommand
            {
                Id = id,
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

        [AllowAnonymous]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudentById(Guid id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

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
