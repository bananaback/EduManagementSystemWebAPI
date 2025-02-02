﻿namespace StudentManagement.Domain.Exceptions
{
    public class InvalidAddressException : Exception
    {
        public InvalidAddressException() { }
        public InvalidAddressException(string message) : base(message) { }
        public InvalidAddressException(string message, Exception innerException) : base(message, innerException) { }
    }
}
