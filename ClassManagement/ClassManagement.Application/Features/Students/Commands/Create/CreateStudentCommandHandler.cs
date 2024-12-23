using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Commands.Create
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
        {
            var existingStudentWithEmail = await _studentRepository.GetByEmailAsync(command.Email, cancellationToken);

            if (existingStudentWithEmail != null)
            {
                throw new StudentCreationException($"Student with email {command.Email} already exist.");
            }

            var newStudent = new Student(
                command.Name,
                command.Email,
                command.EnrollmentDate
            );

            await _studentRepository.AddAsync(newStudent, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentCreationException($"Failed to save changes while creating student.");
            }

            return newStudent.Id;
        }
    }
}
