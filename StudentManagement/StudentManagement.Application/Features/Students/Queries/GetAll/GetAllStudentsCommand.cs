using MediatR;
using StudentManagement.Domain.Enums;

namespace StudentManagement.Application.Features.Students.Queries.GetAll
{
    public class GetAllStudentsCommand : IRequest<IReadOnlyCollection<StudentReadDto>>
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateOnly? DateOfBirthBefore { get; set; }
        public DateOnly? DateOfBirthAfter { get; set; }
        public DateOnly? EnrollmentDateBefore { get; set; }
        public DateOnly? EnrollmentDateAfter { get; set; }
        public string? HouseNumber { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public bool? ExposePrivateInfo { get; set; }
        public int? PageNumber = 1;
        public int? ItemsPerPage = 5;
    }
}
