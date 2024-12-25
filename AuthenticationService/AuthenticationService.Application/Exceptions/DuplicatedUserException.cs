using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Exceptions
{
    public class DuplicatedUserException : Exception
    {
        public DuplicatedUserException() { }
        public DuplicatedUserException(string message) : base(message) { }
        public DuplicatedUserException (string message, Exception innerException) : base(message, innerException) { }   
    }
}
