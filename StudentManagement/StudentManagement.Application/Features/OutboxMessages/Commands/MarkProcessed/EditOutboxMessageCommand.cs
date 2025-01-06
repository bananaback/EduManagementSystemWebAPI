using MediatR;
using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Features.OutboxMessages.Commands.MarkProcessed
{
    public class EditOutboxMessageCommand : IRequest<Guid>
    {
        public Guid MessageId { get; set; }
    }
}
