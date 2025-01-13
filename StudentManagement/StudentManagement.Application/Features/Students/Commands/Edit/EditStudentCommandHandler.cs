using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Application.Features.Students.Commands.Edit
{
    public class EditStudentCommandHandler : IRequestHandler<EditStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EditStudentCommandHandler(IStudentRepository studentRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _outboxRepository = outboxRepository;
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


            // Serialize the updated student object
            var serializedStudent = System.Text.Json.JsonSerializer.Serialize(existingStudent);

            // Create an outbox message for the update
            var message = new OutboxMessage(MessageType.STUDENTUPDATED, serializedStudent);

            // Add the outbox message to the repository
            await _outboxRepository.AddAsync(message, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentPersistenceException("Failed to save changes while updating student.");
            }

            return command.Id;
        }
    }
}
