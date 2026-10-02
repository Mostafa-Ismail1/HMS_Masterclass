using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.HasKey(e => e.AdmissionId).HasName("PK__Admissio__C97EEFA293764032");

        builder.ToTable("Admissions", "clinical");

        builder.Property(e => e.AdmissionId).HasColumnName("AdmissionID");
        builder.Property(e => e.AdmissionDate).HasDefaultValueSql("(sysdatetime())");
        builder.Property(e => e.AdmittingDoctorId).HasColumnName("AdmittingDoctorID");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.RoomId).HasColumnName("RoomID");

        builder.HasOne(d => d.AdmittingDoctor).WithMany(p => p.Admissions)
            .HasForeignKey(d => d.AdmittingDoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Adm_Doctor");

        builder.HasOne(d => d.Patient).WithMany(p => p.Admissions)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Admissions_Patient");

        builder.HasOne(d => d.Room).WithMany(p => p.Admissions)
            .HasForeignKey(d => d.RoomId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Admissions_Room");
    }
}
