using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.Edit
{
    public class EditClassCommandHandler : IRequestHandler<EditClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EditClassCommandHandler(IClassRepository classRepository, IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(EditClassCommand command, CancellationToken cancellationToken = default)
        {
            var exitingClass = await _classRepository.GetByIdAsync(command.Id, cancellationToken);

            if (exitingClass == null)
            {
                throw new ClassRetrievalException($"Class with id {command.Id} not found.");
            }

            exitingClass.Update(command.Name, command.StartDate, command.EndDate);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new ClassPersistenceException($"Failed to save changes while updating class with id {command.Id}");
            }

            return exitingClass.Id;
        }
    }
}
