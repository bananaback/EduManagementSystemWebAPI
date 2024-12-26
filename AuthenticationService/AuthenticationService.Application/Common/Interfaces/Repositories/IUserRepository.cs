using AuthenticationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAll(CancellationToken cancellationToken);
        Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task AddAsync(ApplicationUser user, CancellationToken cancellationToken);

        void Update(ApplicationUser user);
        void Delete(ApplicationUser user);
    }
}
