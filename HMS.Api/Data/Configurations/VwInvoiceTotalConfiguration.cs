using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class VwInvoiceTotalConfiguration : IEntityTypeConfiguration<VwInvoiceTotal>
{
    public void Configure(EntityTypeBuilder<VwInvoiceTotal> builder)
    {
        builder
            .HasNoKey()
            .ToView("vw_InvoiceTotals", "billing");

        builder.Property(e => e.ComputedTotal).HasColumnType("decimal(38, 2)");
        builder.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.Status)
            .HasMaxLength(10)
            .IsUnicode(false);
    }
}
