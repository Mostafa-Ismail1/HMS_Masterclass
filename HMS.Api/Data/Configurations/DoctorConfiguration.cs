using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDFE926E959");

        builder.ToTable("Doctors", "hr");

        builder.HasIndex(e => e.LicenseNumber, "UQ__Doctors__E88901669ECF2ACC").IsUnique();

        builder.Property(e => e.DoctorId)
            .ValueGeneratedNever()
            .HasColumnName("DoctorID");
        builder.Property(e => e.LicenseNumber)
            .HasMaxLength(20)
            .IsUnicode(false);
        builder.Property(e => e.Specialization)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.HasOne(d => d.DoctorNavigation).WithOne(p => p.Doctor)
            .HasForeignKey<Doctor>(d => d.DoctorId)
            .HasConstraintName("FK_Doctors_Staff");
    }
}
