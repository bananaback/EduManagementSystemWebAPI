using ClassManagement.Domain.Common;
using ClassManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Entities
{
    public class InboxMessage : BaseEntity
    {
        public MessageType Type { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string Payload { get; private set; } = string.Empty;
        public bool Processed { get; private set; }

        public InboxMessage() { }
        public InboxMessage(Guid messageId, MessageType type, DateTime dateCreated, string payload)
        {
            Id = messageId;
            Type = type;
            DateCreated = dateCreated;
            Payload = payload;
        }

        public void MarkAsProcessed()
        {
            Processed = true;
        }
    }
}
