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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthenticationDbContext _context;
        public RefreshTokenRepository(AuthenticationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public void Delete(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }

        public async Task DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var userTokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync(cancellationToken);
            _context.RefreshTokens.RemoveRange(userTokens);
        }

        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return _context.RefreshTokens.Where(rt => rt.Token.Value == token).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
