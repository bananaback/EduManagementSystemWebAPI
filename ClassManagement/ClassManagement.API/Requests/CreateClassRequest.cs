using ClassManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassManagement.API.Requests
{
    public class CreateClassRequest : ValidatableRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [EnumDataType(typeof(ClassStatus), ErrorMessage = "Invalid status value.")]
        public ClassStatus Status { get; set; }

        [Range(20, 255, ErrorMessage = "MaxCapacity must be between 20 and 255.")]
        public byte MaxCapacity { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        public CreateClassRequest() { }

        public CreateClassRequest(string name, string description, DateOnly startDate, DateOnly endDate, ClassStatus status, byte maxCapacity)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            MaxCapacity = maxCapacity;
        }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
