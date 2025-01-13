using StudentManagement.API.Validators;
using System.Reflection;

namespace StudentManagement.API.Requests
{
    public abstract class ValidatableRequest
    {
        private RequestValidator? _validator;

        // Set the validator later
        public void Initialize(RequestValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            ValidateProperties();
            Validate(); // Call custom validations
        }

        protected void ValidateProperties()
        {
            if (_validator == null)
            {
                throw new InvalidOperationException("Validator has not been set.");
            }

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(this);

                // Skip null or default values
                if (value == null) continue;

                // Call the validator based on the property's type
                ValidateByType(property.Name, value);
            }
        }

        private void ValidateByType(string propertyName, object value)
        {
            if (_validator == null)
            {
                throw new InvalidOperationException("Validator has not been set.");
            }

            switch (value)
            {
                case string stringValue:
                    _validator.ValidateString(propertyName, stringValue);
                    break;

                case int intValue:
                    _validator.ValidateInt(propertyName, intValue);
                    break;

                // Add more cases for other types as needed
                default:
                    throw new NotSupportedException($"Validation for type {value.GetType().Name} is not supported.");
            }
        }

        // Allow derived classes to add custom validations
        protected abstract void Validate();
    }
}
