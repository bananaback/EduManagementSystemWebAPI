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

        public void Delete(Class @class)
        {
            _context.Remove(@class);
        }

        public Task<List<Class>> GetAll()
        {
            return _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Classes.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Class?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Classes.Where(c => c.Name == name).FirstOrDefaultAsync();
        }

        public void Update(Class @class)
        {
            _context.Update(@class);
        }
    }
}
