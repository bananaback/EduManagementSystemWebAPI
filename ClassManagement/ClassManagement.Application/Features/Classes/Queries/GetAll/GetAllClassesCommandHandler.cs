using ClassManagement.Application.Common.Interfaces;
using ClassManagement.Application.Common.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Queries.GetAll
{
    public class GetAllClassesCommandHandler : IRequestHandler<GetAllClassesCommand, IReadOnlyCollection<ClassReadDto>>
    {
        private readonly IClassRepository _classRepository;
        public GetAllClassesCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<IReadOnlyCollection<ClassReadDto>> Handle(GetAllClassesCommand command, CancellationToken cancellationToken)
        {
            var classes = await _classRepository.SearchAsync(
                command.PageNumber!.Value,
                command.ItemsPerPage!.Value,
                command.Id, 
                command.Name,
                command.Description, 
                command.StartDate, 
                command.EndDate, 
                command.Status, 
                command.MaxCapacity,
                cancellationToken);

            return classes.Select(
                c => new ClassReadDto
                {
                    Id = c.Id,
                    Name = c.Name.Value,
                    Description = c.Description.Value,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    ClassStatus = c.Status,
                    MaxCapacity = c.MaxCapacity
                }
            ).ToList().AsReadOnly();
        }
    }
}
