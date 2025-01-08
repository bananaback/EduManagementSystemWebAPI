using ClassManagement.Domain.Common;
using ClassManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string HouseNumber { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public string Ward { get; private set; } = string.Empty;
        public string District { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;

        private const int MAX_FIELD_LENGTH = 50;

        public Address() { }

        public Address(string houseNumber, string street, string ward, string district, string city)
        {
            // Validate fields not null or empty
            if (string.IsNullOrEmpty(houseNumber))
            {
                throw new InvalidAddressException("House number cannot be null or empty");
            }

            if (string.IsNullOrEmpty(street)) {
                throw new InvalidAddressException("Street cannot be null or empty");
            }

            if (string.IsNullOrEmpty(ward))
            {
                throw new InvalidAddressException("Ward cannot be null or empty");
            }

            if (string.IsNullOrEmpty(district))
            {
                throw new InvalidAddressException("District cannot be null or empty");
            }

            if (string.IsNullOrEmpty(city))
            {
                throw new InvalidAddressException("City cannot be null or empty");
            }

            // Trimmed of leading and trailing white spaces for all fields
            houseNumber = houseNumber.Trim();
            street = street.Trim();
            ward = ward.Trim();
            district = district.Trim();
            city = city.Trim();

            // Validate each field's length in reasonable range
            if (houseNumber.Length > MAX_FIELD_LENGTH)
            {
                throw new InvalidAddressException($"House number must not be more than {MAX_FIELD_LENGTH} characters");
            }

            if (street.Length > MAX_FIELD_LENGTH)
            {
                throw new InvalidAddressException($"Street must not be more than {MAX_FIELD_LENGTH} characters");
            }

            if (ward.Length > MAX_FIELD_LENGTH)
            {
                throw new InvalidAddressException($"Ward must not be more than {MAX_FIELD_LENGTH} characters");
            }

            if (district.Length > MAX_FIELD_LENGTH)
            {
                throw new InvalidAddressException($"District must not be more than {MAX_FIELD_LENGTH} characters");
            }

            if (city.Length > MAX_FIELD_LENGTH)
            {
                throw new InvalidAddressException($"City must not be more than {MAX_FIELD_LENGTH} characters");
            }

            HouseNumber = houseNumber;
            Street = street;
            Ward = ward;
            District = district;
            City = city;
        }

        public string GetFullAddress()
        {
            return $"{HouseNumber}, {Street}, Ward {Ward}, District {District}, {City}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HouseNumber;
            yield return Street;
            yield return Ward;
            yield return District;
            yield return City;
        }
    }
}
