using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Features.OutboxMessages.Commands.MarkProcessed;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/outboxmessages")]
    public class OutboxController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OutboxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id:guid}")]
        public async Task<IActionResult> MarkProcessed(Guid id)
        {
            var command = new EditOutboxMessageCommand
            {
                MessageId = id
            };

            var updatedMessageId = await _mediator.Send(command);

            return Ok($"Mark message with id {updatedMessageId} as processed successfully.");
        }
    }
}
