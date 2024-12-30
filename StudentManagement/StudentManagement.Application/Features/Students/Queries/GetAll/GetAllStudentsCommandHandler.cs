using MediatR;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Features.Students.Queries.GetAll
{
    public class GetAllStudentsCommandHandler : IRequestHandler<GetAllStudentsCommand, IReadOnlyCollection<StudentReadDto>>
    {
        private readonly IStudentRepository _studentRepository;
        public GetAllStudentsCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<IReadOnlyCollection<StudentReadDto>> Handle(GetAllStudentsCommand request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync(cancellationToken);

            return students.Select(
                s => new StudentReadDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    DateEnrolled = s.EnrollmentDate
                }
            ).ToList().AsReadOnly();
        }
    }
}
