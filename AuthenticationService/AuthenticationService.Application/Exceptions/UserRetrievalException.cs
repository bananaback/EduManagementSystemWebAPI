using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Exceptions
{
    public class UserRetrievalException : Exception
    {
        public UserRetrievalException() { }
        public UserRetrievalException(string message) : base(message) { }
        public UserRetrievalException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
