using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Exceptions
{
    public class TokenPersistenceException : Exception
    {
        public TokenPersistenceException() { }
        public TokenPersistenceException(string message) : base(message) { }
        public TokenPersistenceException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
