using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Application.Features.Students.Commands.Edit
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
            var existingStudent = await _studentRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingStudent == null)
            {
                throw new StudentRetrievalException($"Student with id {command.Id} do not exist");
            }

            var personName = (PersonName?)null;
            var email = (Email?)null;

            if (command.Email != null)
            {
                var existingStudentByEmail = await _studentRepository.GetByEmailAsync(command.Email, cancellationToken);

                if (existingStudentByEmail != null)
                {
                    throw new StudentPersistenceException($"Student with email {command.Email} already exist.");
                }

                email = new Email(command.Email);
            }


            if (command.FirstName != null || command.LastName != null)
            {
                personName = new PersonName(command.FirstName!, command.LastName!);
            }

            existingStudent.Update(personName, email, command.Gender, command.DateOfBirth, command.EnrollmentDate, command.Address, command.ExposePrivateInfo);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentPersistenceException("Failed to save changes while updating student.");
            }

            return command.Id;
        }
    }
}
