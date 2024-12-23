using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class ClassCreationException : Exception
    {
        public ClassCreationException() { }
        public ClassCreationException(string message) : base(message) { }
        public ClassCreationException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
