using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.Delete
{
    public class DeleteClassCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
