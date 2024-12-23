using ClassManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAll();
        Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Class @class, CancellationToken cancellationToken);

        void Update(Class @class);
        void Delete(Class @class);

    }
}
