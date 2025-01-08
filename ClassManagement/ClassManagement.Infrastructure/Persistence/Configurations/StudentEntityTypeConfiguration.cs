using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using ClassManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManagement.Infrastructure.Persistence.Configurations
{
    public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");

            builder.Ignore(s => s.DomainEvents);

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(100)
                    .IsRequired();

                name.Property(n => n.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.OwnsOne(s => s.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("email")
                    .HasMaxLength(254)
                    .IsRequired();

                email.HasIndex(e => e.Value)
                    .IsUnique();
            });

            builder.Property(s => s.Gender)
                .HasConversion(
                    v => v.ToString(),
                    v => (GenderEnum)System.Enum.Parse(typeof(GenderEnum), v)
                )
                .HasColumnName("gender")
                .HasMaxLength(50);
            ;

            builder.Property(s => s.DateOfBirth)
                .HasColumnName("date_of_birth")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(s => s.EnrollmentDate)
                .HasColumnName("enrollment_date")
                .HasColumnType("date")
                .IsRequired();

            builder.OwnsOne(s => s.Address, address =>
            {
                address.Property(a => a.HouseNumber)
                    .HasColumnName("house_number")
                    .HasMaxLength(10)
                    .IsRequired();

                address.Property(a => a.Street)
                    .HasColumnName("street")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(a => a.Ward)
                    .HasColumnName("ward")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(a => a.District)
                    .HasColumnName("district")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(a => a.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.Property(s => s.ExposePrivateInfo)
                .HasColumnName("expose_private_info")
                .IsRequired();
        }
    }
}
