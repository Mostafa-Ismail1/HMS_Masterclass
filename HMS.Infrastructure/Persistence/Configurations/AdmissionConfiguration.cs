using HMS.Domain.Entities.Admissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
{
    public void Configure(EntityTypeBuilder<Admission> builder)
    {
        builder.ToTable("Admissions", "clinical");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("AdmissionID");

        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.RoomId).HasColumnName("RoomID");
        builder.Property(e => e.AdmittingDoctorId).HasColumnName("AdmittingDoctorID");

        builder.Property(e => e.AdmissionDate)
            .HasDefaultValueSql("(getdate())");

        builder.Ignore(e => e.DomainEvents);
    }
}
