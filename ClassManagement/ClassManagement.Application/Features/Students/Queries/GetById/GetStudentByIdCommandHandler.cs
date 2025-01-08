using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Queries.GetById
{
    public class GetStudentByIdCommandHandler : IRequestHandler<GetStudentByIdCommand, StudentReadDto>
    {
        private readonly IStudentRepository _studentRepository;
        public GetStudentByIdCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<StudentReadDto> Handle(GetStudentByIdCommand command, CancellationToken cancellationToken = default)
        {
            return new StudentReadDto();
            /*var student = await _studentRepository.GetByIdAsync(command.Id, cancellationToken);

            if (student == null)
            {
                throw new StudentRetrievalException($"Student with id {command.Id} not found.");
            }

            return new StudentReadDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                DateEnrolled = student.EnrollmentDate
            };*/
        }
    }
}
