using StudentManagement.Domain.Commons;
using StudentManagement.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace StudentManagement.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public Email() { }

        public Email(string value)
        {
            // Must not be null
            if (value == null)
            {
                throw new InvalidEmailException("Email must not be null");
            }

            // Trimmed of leading and trailing white spaces
            value = value.Trim();

            // Must not be empty
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidEmailException("Email must not be empty");
            }

            // Max length 254 characters
            if (value.Length > 254)
            {
                throw new InvalidEmailException("Email must not be more than 254 characters");
            }

            // Follow email format
            if (!IsValidEmail(value))
            {
                throw new InvalidEmailException("Email must be in the correct format");
            }

            Value = value;
        }

        private bool IsValidEmail(string email)
        {
            // Use regular expression to validate email format
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
