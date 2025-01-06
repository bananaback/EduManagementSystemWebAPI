using ClassManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces.Repositories
{
    public interface IInboxMessageRepository
    {
        Task<List<InboxMessage>> GetAllAsync(CancellationToken cancellationToken);
        Task<InboxMessage?> GetOldestUnprocessedAsync(CancellationToken cancellationToken);
        Task<InboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(InboxMessage inboxMesage, CancellationToken cancellationToken);

        void Update(InboxMessage inboxMesage);
        void Delete(InboxMessage inboxMesage);
    }
}
