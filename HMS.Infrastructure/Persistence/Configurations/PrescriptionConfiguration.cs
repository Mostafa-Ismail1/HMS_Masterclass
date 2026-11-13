using HMS.Domain.Entities.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescriptions", "clinical");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("PrescriptionID");

        builder.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");

        builder.Property(e => e.DatePrescribed)
            .HasColumnName("DatePrescribed")
            .HasDefaultValueSql("(CONVERT([date],getdate()))");

        builder.Ignore(e => e.DomainEvents);
    }
}
