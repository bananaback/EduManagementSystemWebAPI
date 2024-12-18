using ClassManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventAsync(IEnumerable<BaseEvent> domainEvents, CancellationToken cancellationToken);
    }
}
