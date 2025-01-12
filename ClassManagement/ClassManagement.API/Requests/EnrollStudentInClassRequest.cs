using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassManagement.API.Requests
{
    public class EnrollStudentInClassRequest : ValidatableRequest
    {
        [Required]
        public Guid StudentId { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
