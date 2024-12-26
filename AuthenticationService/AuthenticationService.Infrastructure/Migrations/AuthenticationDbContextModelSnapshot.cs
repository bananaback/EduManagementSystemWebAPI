﻿// <auto-generated />
using System;
using AuthenticationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthenticationService.Infrastructure.Migrations
{
    [DbContext(typeof(AuthenticationDbContext))]
    partial class AuthenticationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthenticationService.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AuthenticationService.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("refresh_tokens", (string)null);
                });

            modelBuilder.Entity("AuthenticationService.Domain.Entities.ApplicationUser", b =>
                {
                    b.OwnsOne("AuthenticationService.Domain.ValueObjects.PasswordHash", "HashPassword", b1 =>
                        {
                            b1.Property<Guid>("ApplicationUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("hash_password");

                            b1.HasKey("ApplicationUserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("ApplicationUserId");
                        });

                    b.OwnsOne("AuthenticationService.Domain.ValueObjects.Username", "Username", b1 =>
                        {
                            b1.Property<Guid>("ApplicationUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("username");

                            b1.HasKey("ApplicationUserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("ApplicationUserId");
                        });

                    b.Navigation("HashPassword")
                        .IsRequired();

                    b.Navigation("Username")
                        .IsRequired();
                });

            modelBuilder.Entity("AuthenticationService.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("AuthenticationService.Domain.Entities.ApplicationUser", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("AuthenticationService.Domain.ValueObjects.TokenValue", "Token", b1 =>
                        {
                            b1.Property<Guid>("RefreshTokenId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("token_value");

                            b1.HasKey("RefreshTokenId");

                            b1.ToTable("refresh_tokens");

                            b1.WithOwner()
                                .HasForeignKey("RefreshTokenId");
                        });

                    b.Navigation("Token")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuthenticationService.Domain.Entities.ApplicationUser", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
