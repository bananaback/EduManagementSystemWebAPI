using ClassManagement.Domain.Common;
using ClassManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.ValueObjects
{
    public class ClassDescription : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public ClassDescription() { }

        public ClassDescription(string value)
        {
            // Must not be null
            if (value == null)
            {
                throw new InvalidClassDescriptionException("Class description must not be null");
            }

            // Trimmed of leading and trailing white spaces
            value = value.Trim();

            // Must not be empty
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidClassDescriptionException("Class description must not be empty");
            }

            // Must not be more than 500 characters
            if (value.Length > 500)
            {
                throw new InvalidClassDescriptionException("Class description must not be more than 500 characters");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
