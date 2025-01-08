using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidAddressException : Exception
    {
        public InvalidAddressException() { }
        public InvalidAddressException(string message) : base(message) { }
        public InvalidAddressException(string message, Exception innerException) : base(message, innerException) { }
    }
}
