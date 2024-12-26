using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.EnrollStudent
{
    public class EnrollStudentCommand : IRequest<bool>
    {
        public Guid ClassId { get; set; }
        public Guid StudentId {  get; set; }
    }
}
