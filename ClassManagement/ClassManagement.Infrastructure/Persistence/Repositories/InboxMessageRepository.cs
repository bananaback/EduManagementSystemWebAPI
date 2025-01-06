using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Persistence.Repositories
{
    public class InboxMessageRepository : IInboxMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public InboxMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(InboxMessage inboxMesage, CancellationToken cancellationToken)
        {
            await _context.InboxMessages.AddAsync(inboxMesage, cancellationToken);
        }

        public void Delete(InboxMessage inboxMesage)
        {
            _context.InboxMessages.Remove(inboxMesage);
        }

        public async Task<List<InboxMessage>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.InboxMessages.ToListAsync(cancellationToken);  
        }

        public async Task<InboxMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.InboxMessages.Where(im => im.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<InboxMessage?> GetOldestUnprocessedAsync(CancellationToken cancellationToken)
        {
            return await _context.InboxMessages
                .Where(im => im.Processed == false)
                .OrderBy(im => im.DateCreated)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public void Update(InboxMessage inboxMesage)
        {
            _context.Update(inboxMesage);
        }
    }
}
