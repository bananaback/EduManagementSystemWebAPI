using MediatR;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.ValueObjects;

namespace StudentManagement.Application.Features.Students.Commands.Edit
{
    public class EditStudentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public Address? Address { get; set; }
        public bool? ExposePrivateInfo { get; set; }
    }
}
