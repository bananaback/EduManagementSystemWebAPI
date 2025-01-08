using ClassManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Commands.Edit
{
    public class EditClassCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }    
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public ClassStatus? Status { get; set; }
        public byte? MaxCapacity { get; set; }
    }
}
