using ClassManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace ClassManagement.API.Requests
{
    public class EditStudentRequest : ValidatableRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public AddressDTO? Address { get; set; }
        public bool? ExposePrivateInfo { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
