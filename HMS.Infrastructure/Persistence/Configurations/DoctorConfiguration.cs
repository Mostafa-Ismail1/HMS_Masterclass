using HMS.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors", "hr");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("DoctorID");

        builder.Property(e => e.Specialization)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.LicenseNumber)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Ignore(e => e.DomainEvents);
    }
}
