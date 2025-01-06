using StudentManagement.Domain.Commons;
using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Entities
{
    public class OutboxMessage : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public MessageType Type { get; private set; }
        public bool Processed { get; private set; } = false;
        public string Payload { get; private set; } = string.Empty;
        public byte[] VersionRow { get; set; } = new byte[0];

        public OutboxMessage()
        {

        }

        public OutboxMessage(MessageType type, string payload)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Type = type;
            Payload = payload;
        }

        public void MarkAsProcessed()
        {
            Processed = true;
        }
    }
}
