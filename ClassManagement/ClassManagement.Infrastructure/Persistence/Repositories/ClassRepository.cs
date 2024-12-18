using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Persistence.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        public ClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Class @class, CancellationToken cancellationToken)
        {
            await _context.AddAsync(@class, cancellationToken);
        }
    }
}
