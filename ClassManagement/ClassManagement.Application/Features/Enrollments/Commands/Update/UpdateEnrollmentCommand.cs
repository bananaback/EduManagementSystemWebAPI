using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Enrollments.Commands.Update
{
    public class UpdateEnrollmentCommand : IRequest<bool>
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public Grade? Grade { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public EnrollmentStatus? Status { get; set; }
    }
}
