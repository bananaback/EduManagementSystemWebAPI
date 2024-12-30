using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Commons.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync(CancellationToken cancellationToken);
        Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Student student, CancellationToken cancellationToken);

        void Update(Student student);
        void Delete(Student student);

    }
}
