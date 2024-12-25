using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.ValueObjects
{
    public class Username : ValueObject
    {
        public string Value { get; private set; } = string.Empty;
        
        private Username() { }

        private Username(string value)
        {
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidUsernameException("Username cannot be empty");
            }

            if (!HasRequiredConstraints(value))
            {
                throw new InvalidUsernameException("User name must have must contains only alphanumeric, underscore or dot. It starts and ends with an alphanumeric character. Length in range 4-30 characters.");
            }

            Value = value;
        }

        public static Username Create(string value)
        {
            return new Username(value);
        }

        private bool HasRequiredConstraints(string username)
        {
            return Regex.IsMatch(username, @"^[a-zA-Z0-9](?!.*\.\.)(?!.*\._)(?!.*_\.)[a-zA-Z0-9._]{2,28}[a-zA-Z0-9]$");
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
