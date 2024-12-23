using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class ClassRetrievalException : Exception
    {
        public ClassRetrievalException() { }
        public ClassRetrievalException(string message) : base(message) { }
        public ClassRetrievalException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
