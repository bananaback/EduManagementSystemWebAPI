using ClassManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassManagement.API.Requests
{
    public class OutboxMessageRequest : ValidatableRequest
    {
        [Required]
        public Guid MessageId { get; set; }
        [Required]
        public MessageType Type { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public string Payload { get; set; } = string.Empty;

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
