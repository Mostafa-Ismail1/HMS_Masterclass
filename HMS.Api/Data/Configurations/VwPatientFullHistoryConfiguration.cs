using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class VwPatientFullHistoryConfiguration : IEntityTypeConfiguration<VwPatientFullHistory>
{
    public void Configure(EntityTypeBuilder<VwPatientFullHistory> builder)
    {
        builder
            .HasNoKey()
            .ToView("vw_PatientFullHistory", "clinical");

        builder.Property(e => e.Dosage)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.LabResult)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.Medication)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.PatientName)
            .HasMaxLength(101)
            .IsUnicode(false);
        builder.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
        builder.Property(e => e.SeenBy)
            .HasMaxLength(101)
            .IsUnicode(false);
    }
}
