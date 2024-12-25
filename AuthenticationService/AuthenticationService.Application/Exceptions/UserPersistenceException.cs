using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Exceptions
{
    public class UserPersistenceException : Exception
    {
        public UserPersistenceException() { }
        public UserPersistenceException(string message) : base(message) { }
        public UserPersistenceException(string message,  Exception innerException) : base(message, innerException) { }  
    }
}
