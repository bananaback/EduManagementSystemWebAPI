using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Commands.Edit
{
    public class EditStudentCommandHandler : IRequestHandler<EditStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public EditStudentCommandHandler(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(EditStudentCommand command, CancellationToken cancellationToken)
        {
            return Guid.NewGuid();
            /*var existingStudent = await _studentRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingStudent == null)
            {
                throw new StudentRetrievalException($"Student with id {command.Id} do not exist");
            }

            if (existingStudent.Name == command.Name
                && existingStudent.Email == command.Email
                && existingStudent.EnrollmentDate == command.EnrollmentDate)
            {
                return command.Id;
            }

            existingStudent.Update(command.Name, command.Email, command.EnrollmentDate);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentPersistenceException("Failed to save changes while updating student.");
            }

            return command.Id;*/
        }
    }
}
