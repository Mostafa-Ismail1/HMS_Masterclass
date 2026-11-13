using HMS.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices", "billing", tb => tb.HasTrigger("trg_LogInvoiceStatusChange"));

        builder.HasKey(e => new { e.InvoiceId, e.IssueDate });

        builder.Property(e => e.InvoiceId)
            .ValueGeneratedOnAdd()
            .HasColumnName("InvoiceID");

        builder.Property(e => e.IssueDate)
            .HasColumnName("IssueDate")
            .HasDefaultValueSql("(CONVERT([date],getdate()))");

        builder.Property(e => e.AdmissionId).HasColumnName("AdmissionID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");

        builder.Property(e => e.Status)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasDefaultValue("Unpaid");

        builder.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
    }
}
