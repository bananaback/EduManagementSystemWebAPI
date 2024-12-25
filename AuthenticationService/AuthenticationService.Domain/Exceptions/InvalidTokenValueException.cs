using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Exceptions
{
    public class InvalidTokenValueException : Exception
    {
        public InvalidTokenValueException() { }
        public InvalidTokenValueException(string message) : base(message) { }
        public InvalidTokenValueException(string message, Exception innerException) : base(message, innerException) { }
    }
}
