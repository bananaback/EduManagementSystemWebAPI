using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Exceptions
{
    public class OutboxMesageRetrievalException : Exception
    {
        public OutboxMesageRetrievalException() { }
        public OutboxMesageRetrievalException(string message) : base(message) { }   
        public OutboxMesageRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}
