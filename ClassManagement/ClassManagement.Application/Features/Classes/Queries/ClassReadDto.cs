using ClassManagement.Application.Features.Classes.Queries.GetById;
using ClassManagement.Application.Features.Students.Queries;
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<EnrollmentReadDto> EnrolledStudents { get; set; } = new List<EnrollmentReadDto>();
    }
}
