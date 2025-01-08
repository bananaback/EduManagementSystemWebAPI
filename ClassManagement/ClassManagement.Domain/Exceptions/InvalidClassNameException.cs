using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.Exceptions
{
    public class InvalidClassNameException : Exception
    {
        public InvalidClassNameException()
        {

        }

        public InvalidClassNameException(string message): base(message)
        {

        }

        public InvalidClassNameException(string message, Exception innerException) : base(message, innerException)
        {

        }   
    }
}
