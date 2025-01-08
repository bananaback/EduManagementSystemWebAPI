using ClassManagement.Application.Features.Classes.Queries.GetById;
using ClassManagement.Application.Features.Students.Queries;
using ClassManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Features.Classes.Queries
{
    public class ClassReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ClassStatus ClassStatus { get; set; }
        public byte MaxCapacity { get; set; }
        public List<EnrollmentReadDto> EnrolledStudents { get; set; } = new List<EnrollmentReadDto>();
    }
}
