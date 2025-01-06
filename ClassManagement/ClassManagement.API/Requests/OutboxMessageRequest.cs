using ClassManagement.Domain.Enums;

namespace ClassManagement.API.Requests
{
    public class OutboxMessageRequest
    {
        public Guid MessageId { get; set; }
        public MessageType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public string Payload { get; set; } = string.Empty;
    }
}
