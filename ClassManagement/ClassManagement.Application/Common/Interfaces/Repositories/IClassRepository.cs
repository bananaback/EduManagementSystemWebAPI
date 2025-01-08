using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Class?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Class @class, CancellationToken cancellationToken);
        void Update(Class @class);
        void Delete(Class @class);

        Task<List<Class>> SearchAsync(
            int pageNumber,
            int itemsPerPage,
            Guid? id = null,
            string? name = null,
            string? description = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            ClassStatus? status = null,
            byte? minCapacity = null,
            CancellationToken cancellationToken = default);
    }
}
