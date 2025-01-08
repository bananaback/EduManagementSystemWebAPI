using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Students.Commands.Edit
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
