using ClassManagement.Domain.Common;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.Events;
using ClassManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Entities
{
    public class Class : BaseEntity
    {
        public ClassName Name { get; private set; } = null!;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public ClassDescription Description { get; private set; } = null!;
        public ClassStatus Status { get; private set; }
        public byte MaxCapacity { get; private set; } 
        private readonly List<Enrollment> _enrollments = new List<Enrollment>();

        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        public IReadOnlyCollection<Student> Students = new List<Student>();

        public Class() { }
        public Class(ClassName name, ClassDescription description, DateTime startDate, DateTime endDate, ClassStatus status, byte maxCapacity)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be greater than start date");
            }

            if (maxCapacity < 20)
            {
                throw new ArgumentException("Max capacity must be greater than 20");
            }

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            MaxCapacity = maxCapacity;
        }

        public void EnrollStudent(Student student)
        {
            if (_enrollments.Any(e => e.StudentId == student.Id))
            {
                throw new InvalidOperationException("Student already enrolled");
            }

            if (_enrollments.Count >= MaxCapacity)
            {
                throw new InvalidOperationException("Class is full");
            }

            AddDomainEvent(new StudentEnrolledEvent(this.Id, student.Id));
            var enrollment = new Enrollment(student, this, DateTime.Now);
            _enrollments.Add(enrollment);
        }

        public void Update(ClassName name, ClassDescription description, DateTime startDate, DateTime endDate, ClassStatus status, byte maxCapacity)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            MaxCapacity = maxCapacity;
        }
    }
}
