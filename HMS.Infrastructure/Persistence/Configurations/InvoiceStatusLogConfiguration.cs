using HMS.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class InvoiceStatusLogConfiguration : IEntityTypeConfiguration<InvoiceStatusLog>
{
    public void Configure(EntityTypeBuilder<InvoiceStatusLog> builder)
    {
        builder.ToTable("InvoiceStatusLog", "billing");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LogID");

        builder.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

        builder.Property(e => e.OldStatus)
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.Property(e => e.NewStatus)
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.Property(e => e.ChangedAt)
            .HasDefaultValueSql("(getdate())");

        builder.Ignore(e => e.DomainEvents);
    }
}
