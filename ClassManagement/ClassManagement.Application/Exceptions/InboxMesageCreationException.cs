using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class InboxMesageCreationException : Exception
    {
        public InboxMesageCreationException() { }
        public InboxMesageCreationException(string message) : base(message) { }
        public InboxMesageCreationException(string message, Exception innerException) : base(message, innerException) { }   
    }
}
