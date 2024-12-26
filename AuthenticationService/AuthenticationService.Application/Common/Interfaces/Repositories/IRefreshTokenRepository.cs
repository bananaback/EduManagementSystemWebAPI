using AuthenticationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        Task DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);    
        void Delete(RefreshToken refreshToken);
    }
}
