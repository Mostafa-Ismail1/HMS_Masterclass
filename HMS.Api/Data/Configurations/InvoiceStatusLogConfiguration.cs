using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class InvoiceStatusLogConfiguration : IEntityTypeConfiguration<InvoiceStatusLog>
{
    public void Configure(EntityTypeBuilder<InvoiceStatusLog> builder)
    {
        builder.HasKey(e => e.LogId).HasName("PK__InvoiceS__5E5499A8ADCF5CEC");

        builder.ToTable("InvoiceStatusLog", "billing");

        builder.Property(e => e.LogId).HasColumnName("LogID");
        builder.Property(e => e.ChangedAt).HasDefaultValueSql("(sysdatetime())");
        builder.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
        builder.Property(e => e.NewStatus)
            .HasMaxLength(10)
            .IsUnicode(false);
        builder.Property(e => e.OldStatus)
            .HasMaxLength(10)
            .IsUnicode(false);
    }
}
