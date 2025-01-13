using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.Features.Students.Queries
{
    public class StudentReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateOnly DateEnrolled { get; set; }
    }
}
