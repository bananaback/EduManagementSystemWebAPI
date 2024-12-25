using AuthenticationService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthenticationDbContext _context;
        public UnitOfWork(AuthenticationDbContext context)
        {
            _context = context;
        }
        
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
