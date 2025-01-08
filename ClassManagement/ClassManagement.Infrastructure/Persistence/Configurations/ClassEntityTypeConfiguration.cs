using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
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

            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.Property(c => c.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(c => c.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date")
                .IsRequired();

            builder.OwnsOne(c => c.Description, description =>
            {
                description.Property(d => d.Value)
                    .HasColumnName("description")
                    .HasMaxLength(500)
                    .IsRequired();
            });

            builder.Property(c => c.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (ClassStatus)Enum.Parse(typeof(ClassStatus), v)
                )
                .HasColumnName("status")
                .HasMaxLength(50);

            builder.Property(c => c.MaxCapacity)
                .HasColumnName("max_capacity")
                .HasColumnType("tinyint")
                .IsRequired();

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

                        j.OwnsOne(e => e.Grade, grade =>
                        {
                            grade.Property(g => g.Value)
                                .HasColumnName("grade")
                                .HasMaxLength(10)
                                .IsRequired();
                        });

                        j.Property(e => e.EnrollmentDate)
                            .HasColumnName("enrollment_date")
                            .HasColumnType("date")
                            .IsRequired();

                        j.Property(e => e.Status)
                            .HasConversion(
                                v => v.ToString(),
                                v => (EnrollmentStatus)Enum.Parse(typeof(EnrollmentStatus), v)
                            )
                            .HasColumnName("status")
                            .HasMaxLength(50);
                    }

                );
        }
    }
}
