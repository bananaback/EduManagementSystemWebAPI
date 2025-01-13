
using StudentManagement.Domain.Commons;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Domain.ValueObjects
{
    public class PersonName : ValueObject
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        private PersonName() { }

        public PersonName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidPersonNameException("First name must not be empty.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidPersonNameException("Last name must not be empty.");

            if (!firstName.All(char.IsLetter) || !lastName.All(char.IsLetter))
                throw new InvalidPersonNameException("Names must only contain alphabetic characters.");

            if ((firstName.Length + lastName.Length) > 50)
                throw new InvalidPersonNameException("Full name must not exceed 50 characters.");

            FirstName = firstName;
            LastName = lastName;
        }

        // Computed property for full name
        public string FullName => $"{FirstName} {LastName}";

        // Factory method to create PersonName from a full name string
        public static PersonName FromFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new InvalidPersonNameException("Full name must not be empty.");

            var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                throw new InvalidPersonNameException("Full name must include both first name and last name.");

            return new PersonName(parts[0], parts[1]);
        }

        // Override ToString
        public override string ToString() => FullName;

        // Equality components
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
