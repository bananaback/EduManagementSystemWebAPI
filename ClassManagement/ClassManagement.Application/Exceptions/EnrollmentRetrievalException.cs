using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Application.Exceptions
{
    public class EnrollmentRetrievalException : Exception
    {
        public EnrollmentRetrievalException() { }
        public EnrollmentRetrievalException(string message) : base(message) { } 
        public EnrollmentRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}
