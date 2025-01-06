using MediatR;
using StudentManagement.Application.Commons.Interfaces;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Exceptions;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentManagement.Application.Features.Students.Commands.Create
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOutboxRepository _outboxMessageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IStudentRepository studentRepository, IOutboxRepository outboxRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _outboxMessageRepository = outboxRepository;
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

            try
            {
                var message = new OutboxMessage(MessageType.STUDENTCREATED, JsonSerializer.Serialize(newStudent));

                await _outboxMessageRepository.AddAsync(message, cancellationToken);

            }
            catch (NotSupportedException ex)
            {
                throw new StudentCreationException($"Failed to create student. {ex.Message}");
            }

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new StudentCreationException($"Failed to save changes while creating student.");
            }

            return newStudent.Id;
        }
    }
}
