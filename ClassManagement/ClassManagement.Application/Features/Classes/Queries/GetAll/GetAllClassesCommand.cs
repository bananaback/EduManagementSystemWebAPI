using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Queries.GetAll
{
    public class GetAllClassesCommand : IRequest<IReadOnlyCollection<ClassReadDto>>
    {

    }
}
