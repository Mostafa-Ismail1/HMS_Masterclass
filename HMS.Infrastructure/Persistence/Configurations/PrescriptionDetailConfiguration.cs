using HMS.Domain.Entities.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class PrescriptionDetailConfiguration : IEntityTypeConfiguration<PrescriptionDetail>
{
    public void Configure(EntityTypeBuilder<PrescriptionDetail> builder)
    {
        builder.ToTable("PrescriptionDetails", "clinical");

        builder.HasKey(e => new { e.PrescriptionId, e.MedicationId });

        builder.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
        builder.Property(e => e.MedicationId).HasColumnName("MedicationID");

        builder.Property(e => e.Dosage)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Duration)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}
