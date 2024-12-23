using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class StudentPersistenceException : Exception
    {
        public StudentPersistenceException() { }
        public StudentPersistenceException(string message) : base(message) { }
        public StudentPersistenceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
