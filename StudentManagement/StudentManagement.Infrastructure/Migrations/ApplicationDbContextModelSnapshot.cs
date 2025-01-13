﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.Infrastructure.Persistence;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentManagement.Domain.Entities.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("payload");

                    b.Property<bool>("Processed")
                        .HasColumnType("bit")
                        .HasColumnName("processed");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("type");

                    b.Property<byte[]>("VersionRow")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("version_row");

                    b.HasKey("Id");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Entities.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<DateOnly>("EnrollmentDate")
                        .HasColumnType("date")
                        .HasColumnName("enrollment_date");

                    b.Property<bool>("ExposePrivateInfo")
                        .HasColumnType("bit")
                        .HasColumnName("expose_private_info");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("gender");

                    b.HasKey("Id");

                    b.ToTable("students", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Entities.Student", b =>
                {
                    b.OwnsOne("StudentManagement.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("city");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("district");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("house_number");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("street");

                            b1.Property<string>("Ward")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("ward");

                            b1.HasKey("StudentId");

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.OwnsOne("StudentManagement.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(254)
                                .HasColumnType("nvarchar(254)")
                                .HasColumnName("email");

                            b1.HasKey("StudentId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.OwnsOne("StudentManagement.Domain.ValueObjects.PersonName", "Name", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("last_name");

                            b1.HasKey("StudentId");

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
