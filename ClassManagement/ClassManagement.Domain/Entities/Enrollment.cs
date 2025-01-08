using ClassManagement.Domain.Common;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
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
        public Grade Grade { get; private set; } = null!;
        public DateTime EnrollmentDate { get; private set; }
        public EnrollmentStatus Status { get; private set; } = EnrollmentStatus.ACTIVE;

        public Enrollment() { }
        public Enrollment(Student student, Class @class, DateTime enrollmentDate, Grade? grade = default)
        {
            if (enrollmentDate > @class.StartDate || enrollmentDate < @class.StartDate.AddDays(-14))
            {
                throw new ArgumentException("Enrollment date should be in 2 weeks prior to start date and before start date");
            }

            Id = Guid.NewGuid();
            Student = student;
            StudentId = student.Id;
            Class = @class;
            ClassId = @class.Id;
            EnrollmentDate = enrollmentDate;
            Grade = grade ?? new Grade("N/A");
        }

        public void Update(Grade grade)
        {
            if (Grade != grade)
            {
                Grade = grade;
            }
        }
    }
}
