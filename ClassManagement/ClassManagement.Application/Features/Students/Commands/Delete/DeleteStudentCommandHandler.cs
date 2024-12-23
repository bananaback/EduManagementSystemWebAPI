using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Commands.Delete
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteStudentCommandHandler(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingStudent == null)
            {
                throw new StudentRetrievalException($"Student with id {command.Id} not exist.");
            }

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
