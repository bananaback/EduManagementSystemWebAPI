using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ClassManagement.API.Requests
{
    public class CreateStudentRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public DateOnly EnrollmentDate { get; set; }
        [Required]
        public AddressDTO Address { get; set; } = null!;
        [Required]
        public bool ExposePrivateInfo { get; set; }

    }
}
