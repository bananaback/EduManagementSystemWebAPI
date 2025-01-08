using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Domain.ValueObjects;
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

            var className = (ClassName?)null;
            var classDescription = (ClassDescription?)null;


            if (command.Name != null)
            {
                var existingClassByName = await _classRepository.GetByNameAsync(command.Name, cancellationToken);
                if (existingClassByName != null)
                {
                    throw new ClassPersistenceException($"Class with name {command.Name} already exist.");
                }

                className = new ClassName(command.Name);

            }

            if (command.Description != null)
            {
                classDescription = new ClassDescription(command.Description);
            }

            if (command.StartDate != null || command.EndDate != null)
            {
                if (command.StartDate > command.EndDate)
                {
                    throw new ClassPersistenceException($"Cannot update class with id {command.Id} because of invalid date range.");
                }
            }

            if (command.MaxCapacity < 20)
            {
                throw new ClassPersistenceException($"Cannot update class with id {command.Id} because class capacity must be greater or equal 20.");
            }

            exitingClass.Update(className, classDescription, command.StartDate, command.EndDate, command.Status, command.MaxCapacity);

            var res = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (res <= 0)
            {
                throw new ClassPersistenceException($"Failed to save changes while updating class with id {command.Id}");
            }

            return exitingClass.Id;
        }
    }
}
