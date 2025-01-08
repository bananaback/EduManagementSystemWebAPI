using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidClassDescriptionException : Exception
    {
        public InvalidClassDescriptionException()
        {
        }

        public InvalidClassDescriptionException(string message) : base(message)
        {
        }

        public InvalidClassDescriptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
