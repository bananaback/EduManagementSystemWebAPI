using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.ValueObjects
{
    public class TokenValue : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public TokenValue() { }
        
        private TokenValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidTokenValueException("Token value cannot be empty.");
            }
            Value = value;
        }

        public static TokenValue Create(string value)
        {
            return new TokenValue(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
