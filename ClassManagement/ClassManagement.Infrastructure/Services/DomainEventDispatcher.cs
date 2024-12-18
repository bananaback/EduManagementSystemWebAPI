using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task DispatchEventAsync(IEnumerable<BaseEvent> domainEvents, 
            CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
