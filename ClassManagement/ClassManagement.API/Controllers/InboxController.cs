using ClassManagement.API.Requests;
using ClassManagement.API.Responses;
using ClassManagement.Application.Features.InboxMessages.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClassManagement.API.Controllers
{
    [ApiController]
    [Route("api/inboxmessages")]
    public class InboxController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InboxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetInboxMessage(OutboxMessageRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (request.AdditionalData != null && request.AdditionalData.Count > 0)
            {
                return BadRequest(new ErrorResponse("Unknown fields detected in the request."));
            }

            var command = new CreateInboxMessageCommand
            {
                MessageId = request.MessageId,
                Type = request.Type,
                DateCreated = request.DateCreated,
                Payload = request.Payload
            };

            var createMessageId = await _mediator.Send(command, cancellationToken);
            return Ok($"Received message with id {createMessageId}");
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
