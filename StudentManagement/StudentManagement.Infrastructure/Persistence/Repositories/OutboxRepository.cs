using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Persistence.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly ApplicationDbContext _context;
        public OutboxRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
        {
            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }

        public void Delete(OutboxMessage outboxMessage)
        {
            _context.OutboxMessages.Remove(outboxMessage);
        }

        public async Task<List<OutboxMessage>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.OutboxMessages.ToListAsync(cancellationToken);
        }
        public void Update(OutboxMessage outboxMessage)
        {
            _context.Update(outboxMessage);
        }

        public async Task<OutboxMessage?> GetOldestUnprocessedAsync(CancellationToken cancellationToken)
        {
            return await _context.OutboxMessages
                .Where(om => om.Processed == false)
                .OrderBy(om => om.CreatedAt).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.OutboxMessages
                .Where(om => om.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
