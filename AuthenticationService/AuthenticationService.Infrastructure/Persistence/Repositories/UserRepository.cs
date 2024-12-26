using AuthenticationService.Application.Common.Interfaces.Repositories;
using AuthenticationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationDbContext _context;
        public UserRepository(AuthenticationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public void Delete(ApplicationUser user)
        {
            _context.Users.Remove(user);
        }

        public Task<List<ApplicationUser>> GetAll(CancellationToken cancellationToken = default)
        {
            return _context.Users.ToListAsync(cancellationToken);
        }

        public Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return _context.Users.Where(u => u.Username.Value == username).FirstOrDefaultAsync(cancellationToken);
        }

        public void Update(ApplicationUser user)
        {
            _context.Update(user);
        }
    }
}
