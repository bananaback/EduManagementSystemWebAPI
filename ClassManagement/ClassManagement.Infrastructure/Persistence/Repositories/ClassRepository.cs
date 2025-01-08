using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
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

        public Task<List<Class>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Classes
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Class?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Classes.Where(c => c.Name.Value == name).FirstOrDefaultAsync();
        }

        public async Task<List<Class>> SearchAsync(
            int pageNumber,
            int itemsPerPage,
            Guid? id = null,
            string? name = null,
            string? description = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            ClassStatus? status = null,
            byte? minCapacity = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Classes.AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(c => c.Id == id.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Value.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(c => c.Description.Value.Contains(description));
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (minCapacity.HasValue)
            {
                query = query.Where(c => c.MaxCapacity >= minCapacity.Value);
            }

            return await query
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(cancellationToken);
        }

        public void Update(Class @class)
        {
            _context.Update(@class);
        }
    }
}
