using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using ClassManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands
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
        public async Task<Guid> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            var newClass = new Class(
                request.ClassName,
                request.StartDate,
                request.EndDate
            );

            await _classRepository.AddAsync(newClass, cancellationToken);

            int result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                throw new Exception("Temp exception");
            }

            return newClass.Id;
        }
    }
}
