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
    public class ClassEntityTypeConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("classes");

            builder.Ignore(c => c.DomainEvents);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id");

            builder.Property(c => c.Name)
                .HasColumnName("name");

            builder.Property(c => c.StartDate)
                .HasColumnName("start_date");


            builder.Property(c => c.EndDate)
                .HasColumnName("end_date");

            builder.HasMany(c => c.Students)
                .WithMany(s => s.Classes)
                .UsingEntity<Enrollment>(
                    j => j.ToTable("enrollments")
                        .HasOne(e => e.Student)
                        .WithMany(s => s.Enrollments)
                        .HasForeignKey(e => e.StudentId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j => j.HasOne(e => e.Class)
                        .WithMany(c => c.Enrollments)
                        .HasForeignKey(e => e.ClassId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j =>
                    {
                        j.Property(j => j.StudentId)
                            .HasColumnName("student_id");
                        
                        j.Property(j => j.ClassId)
                            .HasColumnName("class_id");
                        
                        j.HasKey(e => new {e.StudentId, e.ClassId});

                        j.Property(j => j.Grade)
                            .HasColumnName("grade");

                        j.Property(j => j.EnrollmentDate)
                            .HasColumnName("enrollment_date");
                    }

                );
        }
    }
}
