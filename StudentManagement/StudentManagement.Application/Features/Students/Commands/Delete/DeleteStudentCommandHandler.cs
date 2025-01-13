using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.Features.Students.Commands.Delete
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudentCommandHandler(IStudentRepository studentRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingStudent == null)
            {
                throw new StudentRetrievalException($"Student with id {command.Id} does not exist.");
            }

            // Serialize the student data before deletion
            var serializedStudent = System.Text.Json.JsonSerializer.Serialize(existingStudent);

            // Create an outbox message for the deletion
            var message = new OutboxMessage(MessageType.STUDENTDELETED, serializedStudent);

            // Add the outbox message to the repository
            await _outboxRepository.AddAsync(message, cancellationToken);

            // Proceed with deleting the student
            _studentRepository.Delete(existingStudent);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentPersistenceException($"Failed to save changes while deleting student with id {command.Id}");
            }

            return command.Id;
        }

    }
}
