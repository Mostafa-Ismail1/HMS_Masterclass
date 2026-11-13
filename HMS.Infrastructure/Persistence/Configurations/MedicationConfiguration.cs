using HMS.Domain.Entities.Medications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        builder.ToTable("Medications", "clinical");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("MedicationID");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.Manufacturer)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

        builder.Ignore(e => e.DomainEvents);
    }
}
