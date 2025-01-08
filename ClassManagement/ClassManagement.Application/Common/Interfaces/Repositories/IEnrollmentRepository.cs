using ClassManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Common.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByClassIdAndStudentIdAsync(Guid classId, Guid studentId, CancellationToken cancellationToken);
        void Update(Enrollment enrollment);
    }
}
