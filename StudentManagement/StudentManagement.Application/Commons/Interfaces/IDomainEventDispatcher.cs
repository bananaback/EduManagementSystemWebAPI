using StudentManagement.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Commons.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventAsync(IEnumerable<BaseEvent> domainEvents, CancellationToken cancellationToken);
    }
}
