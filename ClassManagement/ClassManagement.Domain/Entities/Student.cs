using ClassManagement.Domain.Common;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;

namespace ClassManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public PersonName Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public GenderEnum Gender { get; private set; }
        public DateOnly DateOfBirth { get; private set; }

        public DateOnly EnrollmentDate { get; private set; }
        public Address Address { get; private set; } = null!;

        public bool ExposePrivateInfo { get; private set; } = false;


        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
        public IReadOnlyCollection<Class> Classes = new List<Class>();

        public Student() { }

        public Student(PersonName name, Email email, GenderEnum gender, DateOnly dateOfBirth, DateOnly enrollmentDate, Address address, bool exposePrivateInfo)
        {
            if (dateOfBirth.Year - DateTime.Now.Year > 100 || dateOfBirth.Year - DateTime.Now.Year < 5)
            {
                throw new ArgumentException("Invalid date of birth");
            }

            if (enrollmentDate < dateOfBirth)
            {
                throw new ArgumentException("Invalid enrollment date");
            }

            Id = Guid.NewGuid();
            Name = name;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            EnrollmentDate = enrollmentDate;
            Address = address;
            ExposePrivateInfo = exposePrivateInfo;
        }

        public void AllowPrivateInfo()
        {
            ExposePrivateInfo = true;
        }

        public void DisallowPrivateInfo()
        {
            ExposePrivateInfo = false;
        }

        public void Update(PersonName? name = default, Email? email = default, GenderEnum? gender = default, DateOnly? dateOfBirth = default, DateOnly? enrollmentDate = default, Address? address = default, bool? exposePrivateInfo = default)
        {
            if (name != null) Name = name;
            if (email != null) Email = email;
            if (gender.HasValue) Gender = gender.Value;
            if (dateOfBirth.HasValue) DateOfBirth = dateOfBirth.Value;
            if (enrollmentDate.HasValue) EnrollmentDate = enrollmentDate.Value;
            if (address != null) Address = address;
            if (exposePrivateInfo.HasValue) ExposePrivateInfo = exposePrivateInfo.Value;
        }
    }
}
