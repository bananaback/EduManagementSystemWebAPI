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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;
        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }   
        public async Task<Enrollment?> GetByClassIdAndStudentIdAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(e => e.ClassId == classId && e.StudentId == studentId);
        }

        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }
    }
}
