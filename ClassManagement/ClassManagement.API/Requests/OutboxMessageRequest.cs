using ClassManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class OutboxMessageRequest
    {
        [Required]
        public Guid MessageId { get; set; }
        [Required]
        public MessageType Type { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public string Payload { get; set; } = string.Empty;
    }
}
