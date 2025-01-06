using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Persistence.Configurations
{
    public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("outbox_messages");

            builder.Ignore(om => om.DomainEvents);

            builder.HasKey(om => om.Id);

            builder.Property(om => om.Id)
                .HasColumnName("id");

            builder.Property(om => om.Processed)
                .HasColumnName("processed")
                .HasColumnType("bit");

            builder.Property(om => om.VersionRow)
                .IsRowVersion()
                .HasColumnName("version_row");

            builder.Property(om => om.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (MessageType)Enum.Parse(typeof(MessageType), v)
                )
                .HasColumnName("type")
                .HasColumnType("nvarchar(50)");

            builder.Property(om => om.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime2");

            builder.Property(om => om.Payload)
                .HasColumnName("payload")
                .HasColumnType("nvarchar(500)");
        }
    }
}
