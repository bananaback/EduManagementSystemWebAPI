﻿using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Application.Exceptions;
using ClassManagement.Domain.Entities;
using ClassManagement.Domain.ValueObjects;
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
            var existingClassWithName = await _classRepository.GetByNameAsync(command.Name, cancellationToken);

            if (existingClassWithName != null)
            {
                throw new ClassCreationException($"Class with name {command.Name} already exist.");
            }

            var newClass = new Class(
                new ClassName(command.Name),
                new ClassDescription(command.Description),
                command.StartDate,
                command.EndDate,
                command.Status,
                command.MaxCapacity
            );

            await _classRepository.AddAsync(newClass, cancellationToken);

            int result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                throw new ClassCreationException("Failed to save changes when creating new class");
            }

            return newClass.Id;

        }
    }
}
