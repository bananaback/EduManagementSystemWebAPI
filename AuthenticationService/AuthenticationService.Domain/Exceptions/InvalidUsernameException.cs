using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() { }
        public InvalidUsernameException(string message) : base(message) { }
        public InvalidUsernameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
