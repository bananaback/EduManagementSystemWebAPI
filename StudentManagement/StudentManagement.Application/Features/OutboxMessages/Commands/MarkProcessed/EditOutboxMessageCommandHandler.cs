using MediatR;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Features.OutboxMessages.Commands.MarkProcessed
{
    public class EditOutboxMessageCommandHandler : IRequestHandler<EditOutboxMessageCommand, Guid>
    {
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EditOutboxMessageCommandHandler> _logger;
        
        public EditOutboxMessageCommandHandler(IOutboxRepository outboxRepository, IUnitOfWork unitOfWork, ILogger<EditOutboxMessageCommandHandler> logger)
        {
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(EditOutboxMessageCommand request, CancellationToken cancellationToken)
        {
            var existingMessage = await _outboxRepository.GetByIdAsync(request.MessageId, cancellationToken);

            if (existingMessage == null)
            {
                throw new OutboxMesageRetrievalException($"Outbox message with id {request.MessageId} not found.");
            }

            if (existingMessage.Processed == true)
            {
                _logger.Log(LogLevel.Warning, $"Outbox message with id {request.MessageId} already processed.");
                throw new OutboxMessagePersistenceException($"Message with id {request.MessageId} already processed.");
            }

            existingMessage.MarkAsProcessed();

            try
            {
                var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (res <= 0)
                {
                    throw new OutboxMessagePersistenceException($"Failed to mark outbox message with id {request.MessageId} as processed.");
                }

                return existingMessage.Id;
            }
            catch (DBConcurrencyException ex)
            {
                _logger.Log(LogLevel.Error, ex, "Failed to mark outbox message as processed.");
                throw new OutboxMessagePersistenceException($"Failed to mark outbox message with id {request.MessageId} as processed.");
            }
        }
    }
}
