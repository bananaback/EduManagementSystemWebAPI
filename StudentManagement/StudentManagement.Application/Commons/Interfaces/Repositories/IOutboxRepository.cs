using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Commons.Interfaces.Repositories
{
    public interface IOutboxRepository
    {
        Task<List<OutboxMessage>> GetAllAsync(CancellationToken cancellationToken);
        Task<OutboxMessage?> GetOldestUnprocessedAsync(CancellationToken cancellationToken);
        Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken);

        void Update(OutboxMessage outboxMessage);
        void Delete(OutboxMessage outboxMessage);
    }
}
