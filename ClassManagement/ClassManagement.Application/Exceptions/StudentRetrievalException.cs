using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class StudentRetrievalException : Exception
    {
        public StudentRetrievalException() { }
        public StudentRetrievalException(string message) : base(message) { }
        public StudentRetrievalException(string message, Exception innerExpception) : base(message, innerExpception) { }
    }
}
