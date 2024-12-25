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
    public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_token");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id)
                .HasColumnName("id");

            builder.Property(rt => rt.Token)
                .HasConversion(
                    tokenValue => tokenValue.Value,
                    value => TokenValue.Create(value)
                )
                .HasColumnName("token");

            builder.Property(rt => rt.UserId)
                .HasColumnName("user_id");
        }
    }
}
