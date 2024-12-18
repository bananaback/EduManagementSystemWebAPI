using ClassManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Persistence.Configurations
{
    public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.Name)
                .HasColumnName("name");

            builder.Property(s => s.Email)
                .HasColumnName("email");

            builder.Property(s => s.EnrollmentDate)
                .HasColumnName("enrollment_date");
        }
    }
}
