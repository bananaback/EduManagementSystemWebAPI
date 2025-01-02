using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.ValueObjects
{
    public class PasswordHash : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public PasswordHash() { }

        private PasswordHash(string value)
        {
            if (value == null)
            {
                throw new InvalidPasswordHashException("Password hash cannot be null or empty.");
            }

            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidPasswordHashException("Password hash cannot be null or empty.");
            }

            if (!IsValidBcryptPasswordHash(value))
            {
                throw new InvalidPasswordHashException("Invalid bcrypt format.");
            }

            Value = value;
        }

        private bool IsValidBcryptPasswordHash(string hash)
        {
            return hash.Length == 60 && hash.StartsWith("$");
        }

        public static PasswordHash Create(string value)
        {
            return new PasswordHash(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
