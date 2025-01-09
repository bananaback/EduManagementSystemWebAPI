using ClassManagement.Domain.Common;
using ClassManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.ValueObjects
{
    public class Grade : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        public Grade() { }

        public Grade(string value)
        {
            if (value == null)
            {
                throw new InvalidGradeException("Grade must not be null");
            }

            value = value.ToUpper();
            
            Value = value.Trim();
            
            if (Value != "N/A" && Value != "A+" && Value != "A" && Value != "A-" && Value != "B+" && Value != "B" && Value != "B-" && Value != "C+" && Value != "C" && Value != "C-" && Value != "D+" && Value != "D" && Value != "D-" && Value != "F")
            {
                throw new InvalidGradeException("Grade must be A+, A, A-, B+, B, B-, C+, C, C-, D+, D, D-, F, or N/A");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
