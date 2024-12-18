using ClassManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public DateTime EnrollmentDate { get; private set; }
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        public IReadOnlyCollection<Class> Classes = new List<Class>();

        public Student() { }

        public Student(string name, string email, DateTime enrollmentDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            EnrollmentDate = enrollmentDate;
        }
    }
}
