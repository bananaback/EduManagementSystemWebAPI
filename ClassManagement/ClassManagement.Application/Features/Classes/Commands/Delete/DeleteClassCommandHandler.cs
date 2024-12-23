using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.Delete
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteClassCommandHandler(IClassRepository classRepository, IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteClassCommand command, CancellationToken cancellationToken = default)
        {
            var existingClass = await _classRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingClass == null)
            {
                throw new ClassRetrievalException($"Class with id {command.Id} not found.");
            }

            _classRepository.Delete(existingClass);
            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new ClassPersistenceException($"Failed to save changes while deleting class with id {command.Id}");
            }
            return command.Id;
        }
    }
}
