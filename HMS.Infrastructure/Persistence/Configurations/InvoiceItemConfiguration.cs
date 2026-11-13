using HMS.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("InvoiceItems", "billing");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("InvoiceItemID");

        builder.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
        builder.Property(e => e.IssueDate).HasColumnName("IssueDate");

        builder.Property(e => e.Description)
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

        builder.Ignore(e => e.DomainEvents);
    }
}
