using MediatR;
using StudentManagement.Application.Commons.Interfaces.Repositories;

namespace StudentManagement.Application.Features.Students.Queries.GetAll
{
    public class GetAllStudentsCommandHandler : IRequestHandler<GetAllStudentsCommand, IReadOnlyCollection<StudentReadDto>>
    {
        private readonly IStudentRepository _studentRepository;
        public GetAllStudentsCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<IReadOnlyCollection<StudentReadDto>> Handle(GetAllStudentsCommand command, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync(
                command.PageNumber!.Value,
                command.ItemsPerPage!.Value,
                command.Id,
                command.FirstName,
                command.LastName,
                command.Email,
                command.DateOfBirthBefore,
                command.DateOfBirthAfter,
                command.EnrollmentDateBefore,
                command.EnrollmentDateAfter,
                command.HouseNumber,
                command.Street,
                command.Ward,
                command.District,
                command.City,
                cancellationToken);

            return students.Select(
                student => new StudentReadDto
                {
                    Id = student.Id,
                    Name = student.ExposePrivateInfo ? student.Name.FullName : "secret",
                    Email = student.ExposePrivateInfo ? student.Email.Value : "secret",
                    DateOfBirth = student.ExposePrivateInfo ? student.DateOfBirth : DateOnly.MinValue,
                    DateEnrolled = student.EnrollmentDate,
                    Address = student.ExposePrivateInfo ? student.Address.GetFullAddress() : "secret"
                }
            ).ToList().AsReadOnly();
        }
    }
}
