using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.ValueObjects
{
    public class Password : ValueObject
    {
        public string Value { get; private set; } = string .Empty; 
        
        public Password() { }

        private Password(string value)
        {
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidPasswordException("Password cannot be empty.");
            }

            if (!HasRequiredStrength(value))
            {
                throw new InvalidPasswordException("Password must contains atleast one uppercase, one lowercase, one digit and one special character with a length in range 8-64, no space is allowed.");
            }

            Value = value;
        }

        public static Password Create(string value)
        {
            return new Password(value); 
        }

        private bool HasRequiredStrength(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,64}$");
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
