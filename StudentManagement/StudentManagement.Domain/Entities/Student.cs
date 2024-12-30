using StudentManagement.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public DateTime EnrollmentDate { get; private set; }

        public Student() { }

        public Student(string name, string email, DateTime enrollmentDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            EnrollmentDate = enrollmentDate;
        }

        public void Update(string name, string email, DateTime enrollmentDate)
        {
            Name = name;
            Email = email;
            EnrollmentDate = enrollmentDate;
        }
    }
}
