using ClassManagement.Domain.Entities;
using ClassManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClassManagement.Infrastructure.Persistence.Configurations
{
    public class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("inbox_messages");

            builder.HasKey(im => im.Id);

            builder.Property(im => im.Id)
                .HasColumnName("id");

            builder.Ignore(im => im.DomainEvents);

            builder.Property(im => im.Type)
                .HasConversion(v => v.ToString(), v => (MessageType)Enum.Parse(typeof(MessageType), v))
                .HasColumnName("message_type")
                .HasMaxLength(50);

            builder.Property(im => im.DateCreated)
                .HasColumnType("datetime2")
                .HasColumnName("date_created");

            builder.Property(im => im.Payload)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("payload");

            builder.Property(im => im.Processed)
                .HasColumnName("processed");
        }
    }
}
