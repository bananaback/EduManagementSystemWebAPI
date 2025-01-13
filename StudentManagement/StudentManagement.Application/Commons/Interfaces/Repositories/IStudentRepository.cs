using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Commons.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync(
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
            CancellationToken cancellationToken);
        Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Student student, CancellationToken cancellationToken);

        void Update(Student student);
        void Delete(Student student);

    }
}
