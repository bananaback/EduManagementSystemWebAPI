using StudentManagement.Domain.Commons;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Events
{
    public class StudentCreatedEvent : BaseEvent
    {
        public DomainEventType Type { get; private set; }
        public Student Student { get; private set; }

        public StudentCreatedEvent(Student student)
        {
            Type = DomainEventType.STUDENTCREATED;
            Student = student;
        }
    }
}
