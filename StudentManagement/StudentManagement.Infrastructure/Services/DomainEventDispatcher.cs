using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Services
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
