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
        

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
