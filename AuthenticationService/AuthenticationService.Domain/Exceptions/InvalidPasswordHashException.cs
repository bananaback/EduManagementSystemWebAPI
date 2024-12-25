using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Exceptions
{
    public class InvalidPasswordHashException : Exception
    {
        public InvalidPasswordHashException() { }
        public InvalidPasswordHashException(string message) : base(message) { }
        public InvalidPasswordHashException(string message, Exception innerException) : base(message, innerException) { }
    }
}
