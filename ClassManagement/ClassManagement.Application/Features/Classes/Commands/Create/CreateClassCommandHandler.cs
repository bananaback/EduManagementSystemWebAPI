using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.Create
{
    public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateClassCommandHandler(IClassRepository classRepository,
            IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateClassCommand command, CancellationToken cancellationToken)
        {
            return Guid.NewGuid();
            /*var existingClassWithName = await _classRepository.GetByNameAsync(command.ClassName, cancellationToken);

            if (existingClassWithName != null)
            {
                throw new ClassCreationException($"Class with name {command.ClassName} already exist.");
            }

            var newClass = new Class(
                command.ClassName,
                command.StartDate,
                command.EndDate
            );

            await _classRepository.AddAsync(newClass, cancellationToken);

            int result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                throw new ClassCreationException("Failed to save changes when creating new class");
            }

            return newClass.Id;*/

        }
    }
}
