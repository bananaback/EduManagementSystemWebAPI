using ClassManagement.Domain.Common;
using ClassManagement.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Entities
{
    public class Class : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();

        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        public IReadOnlyCollection<Student> Students = new List<Student>();

        public Class() { }
        public Class(string name, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void EnrollStudent(Student student)
        {
            AddDomainEvent(new StudentEnrolledEvent(this.Id, student.Id));
            var enrollment = new Enrollment(student, this, DateTime.Now);
        }

        public void Update(string name, DateTime startDate, DateTime endDate)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
