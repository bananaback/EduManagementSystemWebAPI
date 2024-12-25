using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("user");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.OwnsOne(u => u.Username, username =>
            {
                username.Property(u => u.Value)
                    .HasColumnName("username")
                    .IsRequired();
            });

            builder.OwnsOne(u => u.HashPassword, hashPassword =>
            {
                hashPassword.Property(h => h.Value)
                    .HasColumnName("hash_password")
                    .IsRequired();
            });

            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
