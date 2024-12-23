using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class ClassPersistenceException : Exception
    {
        public ClassPersistenceException() { }
        public ClassPersistenceException(string message) : base(message) { }
        public ClassPersistenceException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
