using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidGradeException : Exception
    {
        public InvalidGradeException() { }
        public InvalidGradeException(string message) : base(message) { }
        public InvalidGradeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
