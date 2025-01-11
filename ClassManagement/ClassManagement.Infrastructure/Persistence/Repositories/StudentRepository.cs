using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassManagement.Infrastructure.Persistence.Repositories
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

        public async Task<List<Student>> GetAllAsync(
            int pageNumber,
            int itemsPerPage,
            Guid? id,
            string? firstName,
            string? lastName,
            string? email,
            DateOnly? dateOfBirthBefore,
            DateOnly? dateOfBirthAfter,
            DateOnly? enrollmentDateBefore,
            DateOnly? enrollmentDateAfter,
            string? houseNumber,
            string? street,
            string? ward,
            string? district,
            string? city,
            CancellationToken cancellationToken)
        {
            var query = _context.Students.AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(s => s.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Name.FirstName == firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Name.LastName == lastName);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Email.Value == email);
            }

            if (dateOfBirthBefore.HasValue)
            {
                query = query.Where(s => s.ExposePrivateInfo && s.DateOfBirth <= dateOfBirthBefore);
            }

            if (dateOfBirthAfter.HasValue)
            {
                query = query.Where(s => s.ExposePrivateInfo && s.DateOfBirth >= dateOfBirthAfter);
            }

            if (!string.IsNullOrWhiteSpace(houseNumber))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Address.HouseNumber == houseNumber);
            }

            if (!string.IsNullOrWhiteSpace(street))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Address.Street == street);
            }

            if (!string.IsNullOrWhiteSpace(ward))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Address.Ward == ward);
            }

            if (!string.IsNullOrWhiteSpace(district))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Address.District == district);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(s => s.ExposePrivateInfo && s.Address.City == city);
            }

            if (enrollmentDateBefore.HasValue)
            {
                query = query.Where(s => s.EnrollmentDate <= enrollmentDateBefore);
            }

            if (enrollmentDateAfter.HasValue)
            {
                query = query.Where(s => s.EnrollmentDate >= enrollmentDateAfter);
            }

            return await query
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(cancellationToken);
        }

        public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Students.Where(s => s.Email.Value == email).FirstOrDefaultAsync();
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
