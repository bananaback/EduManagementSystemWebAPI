using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Exceptions
{
    public class OutboxMessagePersistenceException : Exception
    {
        public OutboxMessagePersistenceException() { }
        public OutboxMessagePersistenceException(string message) : base(message) { }
        public OutboxMessagePersistenceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
