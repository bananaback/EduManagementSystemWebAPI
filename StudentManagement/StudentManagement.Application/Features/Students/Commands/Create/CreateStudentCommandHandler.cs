using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Application.Features.Students.Commands.Create
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IStudentRepository studentRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
        {
            var existingStudentWithEmail = await _studentRepository.GetByEmailAsync(command.Email, cancellationToken);

            if (existingStudentWithEmail != null)
            {
                throw new StudentCreationException($"Student with email {command.Email} already exist.");
            }

            var name = new PersonName(command.FirstName, command.LastName);
            var email = new Email(command.Email);

            var newStudent = new Student(
                name,
                email,
                command.Gender,
                command.DateOfBirth,
                command.EnrollmentDate,
                command.Address,
                command.ExposePrivateInfo
            );

            await _studentRepository.AddAsync(newStudent, cancellationToken);

            // Serialize the student object
            var serializedStudent = System.Text.Json.JsonSerializer.Serialize(newStudent);

            // Pass serializedStudent to the OutboxMessage constructor
            var message = new OutboxMessage(MessageType.STUDENTCREATED, serializedStudent);

            await _outboxRepository.AddAsync(message, cancellationToken);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentCreationException($"Failed to save changes while creating student.");
            }

            return newStudent.Id;
        }
    }
}
