using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.HasKey(e => e.PrescriptionId).HasName("PK_PrescriptionID");

        builder.ToTable("Prescriptions", "clinical");

        builder.HasIndex(e => e.AppointmentId, "UQ__Prescrip__8ECDFCA39E2C5272").IsUnique();

        builder.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
        builder.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
        builder.Property(e => e.DatePrescribed).HasDefaultValueSql("(CONVERT([date],getdate()))");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");

        builder.HasOne(d => d.Appointment).WithOne(p => p.Prescription)
            .HasForeignKey<Prescription>(d => d.AppointmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Presc_Appointment");

        builder.HasOne(d => d.Doctor).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Presc_Doctor");

        builder.HasOne(d => d.Patient).WithMany(p => p.Prescriptions)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Presc_Patient");
    }
}
