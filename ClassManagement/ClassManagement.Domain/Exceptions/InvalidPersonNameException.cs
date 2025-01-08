using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidPersonNameException : Exception
    {
        public InvalidPersonNameException() { }
        public InvalidPersonNameException(string message) : base(message) { }
        public InvalidPersonNameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
