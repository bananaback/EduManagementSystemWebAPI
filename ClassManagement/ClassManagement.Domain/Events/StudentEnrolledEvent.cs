using ClassManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Events
{
    public class StudentEnrolledEvent : BaseEvent
    {
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public StudentEnrolledEvent(Guid classId, Guid studentId)
        {
            ClassId = classId;
            StudentId = studentId;
        }
    }
}
