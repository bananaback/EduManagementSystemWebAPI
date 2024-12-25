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
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidPasswordHashException("Password hash cannot be empty");
            }
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
