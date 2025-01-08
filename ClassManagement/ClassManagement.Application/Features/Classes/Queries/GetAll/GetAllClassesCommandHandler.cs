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

        public async Task<IReadOnlyCollection<ClassReadDto>> Handle(GetAllClassesCommand request, CancellationToken cancellationToken)
        {
            return new List<ClassReadDto>().AsReadOnly();
            /*var classes = await _classRepository.GetAll();
            
            return classes.Select(
                c => new ClassReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                }    
            ).ToList().AsReadOnly();*/
        }
    }
}
