using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        builder.HasKey(e => e.MedicationId).HasName("PK__Medicati__62EC1ADAEDF5FA50");

        builder.ToTable("Medications", "clinical");

        builder.Property(e => e.MedicationId).HasColumnName("MedicationID");
        builder.Property(e => e.Manufacturer)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
    }
}
