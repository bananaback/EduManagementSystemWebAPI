using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Persistence.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student, CancellationToken cancellationToken)
        {
            await _context.Students.AddAsync(student, cancellationToken);
        }

        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }

        public Task<List<Student>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Students.ToListAsync(cancellationToken);
        }

        public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Students.Where(s => s.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
        }
    }
}
