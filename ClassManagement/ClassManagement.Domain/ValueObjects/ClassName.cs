using ClassManagement.Domain.Common;
using ClassManagement.Domain.Exceptions;

namespace ClassManagement.Domain.ValueObjects
{
    public class ClassName : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public ClassName() { }

        public ClassName(string value)
        {
            // Must not be null
            if (value == null)
            {
                throw new InvalidClassNameException("Class name must not be null");
            }

            // Trimmed of leading and trailing white spaces
            value = value.Trim();

            // Must not be empty
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidClassNameException("Class name must not be empty");
            }

            // Max length of 100 characters
            if (value.Length > 100)
            {
                throw new InvalidClassNameException("Class name must not be more than 100 characters");
            }

            // No special characters or numbers
            if (!value.All(char.IsLetter))
            {
                throw new InvalidClassNameException("Class name must not contain special characters or numbers");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
