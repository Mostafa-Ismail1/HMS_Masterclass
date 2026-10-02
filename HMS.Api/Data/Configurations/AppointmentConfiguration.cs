using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2B3D6CDE6");

        builder.ToTable("Appointments", "clinical", tb => tb.HasTrigger("trg_PreventOverbooking"));

        builder.HasIndex(e => e.AppointmentDate, "IX_Appointments_Date");

        builder.HasIndex(e => e.AppointmentDate, "IX_Appointments_Scheduled_Filtered").HasFilter("([Status]='Scheduled')");

        builder.HasIndex(e => new { e.DoctorId, e.AppointmentDate }, "UQ_Doctor_Slot").IsUnique();

        builder.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.Reason)
            .HasMaxLength(200)
            .IsUnicode(false);
        builder.Property(e => e.Status)
            .HasMaxLength(15)
            .IsUnicode(false)
            .HasDefaultValue("Scheduled");

        builder.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_App_Doctor");

        builder.HasOne(d => d.Patient).WithMany(p => p.Appointments)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointments_Patient");
    }
}
