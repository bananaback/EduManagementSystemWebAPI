using ClassManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public Student Student { get; private set; } = null!;
        public Guid ClassId { get; private set; }
        public Class Class { get; private set; } = null!;
        public string Grade { get; private set; } = string.Empty;
        public DateTime EnrollmentDate { get; private set; }

        public Enrollment() { }
        public Enrollment(Student student, Class @class, DateTime enrollmentDate)
        {
            Id = Guid.NewGuid();
            Student = student;
            StudentId = student.Id;
            Class = @class;
            ClassId = @class.Id;
            EnrollmentDate = enrollmentDate;
        }

        public void UpdateGrade(string grade)
        {
            Grade = grade;
        }
    }
}
