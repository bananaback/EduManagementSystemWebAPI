using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class StudentEnrollmentException : Exception
    {
        public StudentEnrollmentException() { }
        public StudentEnrollmentException(string message) : base(message) { }
        public StudentEnrollmentException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
