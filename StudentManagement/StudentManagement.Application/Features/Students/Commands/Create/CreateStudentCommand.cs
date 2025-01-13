using MediatR;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Application.Features.Students.Commands.Create
{
    public class CreateStudentCommand : IRequest<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public Address Address { get; set; } = null!;
        public bool ExposePrivateInfo { get; set; }
    }
}
