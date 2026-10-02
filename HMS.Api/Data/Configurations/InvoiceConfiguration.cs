using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(e => new { e.InvoiceId, e.IssueDate });

        builder.ToTable("Invoices", "billing", tb => tb.HasTrigger("trg_LogInvoiceStatusChange"));

        builder.HasIndex(e => new { e.PatientId, e.IssueDate }, "IX_Invoices_Patient");

        builder.Property(e => e.InvoiceId)
            .ValueGeneratedOnAdd()
            .HasColumnName("InvoiceID");
        builder.Property(e => e.IssueDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
        builder.Property(e => e.AdmissionId).HasColumnName("AdmissionID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.Status)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasDefaultValue("Unpaid");
        builder.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

        builder.HasOne(d => d.Admission).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.AdmissionId)
            .HasConstraintName("FK_Inv_Admission");

        builder.HasOne(d => d.Patient).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inv_Patient");
    }
}
