using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() { }
        public InvalidEmailException(string message) : base(message) { }
        public InvalidEmailException(string message, Exception innerException) : base(message, innerException) { }
    }
}
