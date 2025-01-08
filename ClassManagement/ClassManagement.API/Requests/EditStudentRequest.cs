using ClassManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class EditStudentRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public AddressDTO? Address { get; set; }
        public bool? ExposePrivateInfo { get; set; }
    }
}
