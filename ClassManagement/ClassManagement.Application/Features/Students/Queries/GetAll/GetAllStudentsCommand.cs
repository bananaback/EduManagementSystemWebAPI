using ClassManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Queries.GetAll
{
    public class GetAllStudentsCommand : IRequest<IReadOnlyCollection<StudentReadDto>>
    {
        
    }
}
