using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Features.Students.Queries.GetById
{
    public class GetStudentByIdCommand : IRequest<StudentReadDto>
    {
        public Guid Id { get; set; }
    }
}
