using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.HasKey(e => e.InvoiceItemId).HasName("PK__InvoiceI__478FE0FCB15C9B86");

        builder.ToTable("InvoiceItems", "billing");

        builder.Property(e => e.InvoiceItemId).HasColumnName("InvoiceItemID");
        builder.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.Description)
            .HasMaxLength(200)
            .IsUnicode(false);
        builder.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

        builder.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
            .HasForeignKey(d => new { d.InvoiceId, d.IssueDate })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Items_Invoice");
    }
}
