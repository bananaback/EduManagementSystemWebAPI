namespace StudentManagement.Domain.Exceptions
{
    public class InvalidPersonNameException : Exception
    {
        public InvalidPersonNameException() { }
        public InvalidPersonNameException(string message) : base(message) { }
        public InvalidPersonNameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
