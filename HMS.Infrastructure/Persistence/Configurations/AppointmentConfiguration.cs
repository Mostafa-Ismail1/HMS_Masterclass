using HMS.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments", "clinical", tb => tb.HasTrigger("trg_PreventOverbooking"));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("AppointmentID");

        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");

        builder.Property(e => e.AppointmentDate)
            .HasColumnName("AppointmentDate");

        builder.Property(e => e.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsUnicode(false);

        builder.Property(e => e.Reason)
            .HasColumnName("Reason")
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Ignore(e => e.CreatedAtUtc);
        builder.Ignore(e => e.DomainEvents);
    }
}
