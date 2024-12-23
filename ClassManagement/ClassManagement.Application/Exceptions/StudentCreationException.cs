using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class StudentCreationException : Exception
    {
        public StudentCreationException() { }
        public StudentCreationException(string message) : base(message) { }
        public StudentCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
