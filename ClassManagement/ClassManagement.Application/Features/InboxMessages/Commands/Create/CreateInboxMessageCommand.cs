using ClassManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.InboxMessages.Commands.Create
{
    public class CreateInboxMessageCommand : IRequest<Guid>
    {
        public Guid MessageId { get; set; }
        public MessageType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public string Payload { get; set; } = string.Empty;
    }
}
