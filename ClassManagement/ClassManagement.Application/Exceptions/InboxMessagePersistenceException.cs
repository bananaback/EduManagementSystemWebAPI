using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class InboxMessagePersistenceException : Exception
    {
        public InboxMessagePersistenceException() { }
        public InboxMessagePersistenceException(string message) : base(message) { }
        public InboxMessagePersistenceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
