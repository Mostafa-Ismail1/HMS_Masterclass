using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class PrescriptionDetailConfiguration : IEntityTypeConfiguration<PrescriptionDetail>
{
    public void Configure(EntityTypeBuilder<PrescriptionDetail> builder)
    {
        builder.HasKey(e => new { e.PrescriptionId, e.MedicationId }).HasName("PK_PrescDetails");

        builder.ToTable("PrescriptionDetails", "clinical");

        builder.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
        builder.Property(e => e.MedicationId).HasColumnName("MedicationID");
        builder.Property(e => e.Dosage)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.Duration)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.HasOne(d => d.Medication).WithMany(p => p.PrescriptionDetails)
            .HasForeignKey(d => d.MedicationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PD_Medication");

        builder.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
            .HasForeignKey(d => d.PrescriptionId)
            .HasConstraintName("FK_PD_Prescription");
    }
}
