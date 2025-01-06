using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Common.Interfaces.Services;
using ClassManagement.Application.Exceptions;
using ClassManagement.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;

namespace ClassManagement.Application.Features.InboxMessages.Commands.Create
{
    public class CreateInboxMessageCommandHandler : IRequestHandler<CreateInboxMessageCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInboxMessageRepository _inboxMessageRepository;
        private readonly IMessageSender _messageSender;
        private readonly ILogger<CreateInboxMessageCommandHandler> _logger;
        public CreateInboxMessageCommandHandler(IUnitOfWork unitOfWork, IInboxMessageRepository inboxMessageRepository, IMessageSender messageSender, ILogger<CreateInboxMessageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _inboxMessageRepository = inboxMessageRepository;
            _messageSender = messageSender;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateInboxMessageCommand request, CancellationToken cancellationToken)
        {
            var existingMessage = await _inboxMessageRepository.GetByIdAsync(request.MessageId, cancellationToken);

            if (existingMessage != null)
            {
                _logger.LogInformation($"Message with id {request.MessageId} already exists in the inbox, acknowledging sender...");
                await _messageSender.AckMessageReceivedAsync(request.MessageId, cancellationToken);
            }

            var inboxMessage = new InboxMessage(request.MessageId, request.Type, request.DateCreated, request.Payload);

            await _inboxMessageRepository.AddAsync(inboxMessage, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new InboxMessagePersistenceException($"Failed to save changes while creating inbox message with id {request.MessageId}.");
            }

            return inboxMessage.Id;
        }
    }
}
