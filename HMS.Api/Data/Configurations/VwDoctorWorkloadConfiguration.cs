using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class VwDoctorWorkloadConfiguration : IEntityTypeConfiguration<VwDoctorWorkload>
{
    public void Configure(EntityTypeBuilder<VwDoctorWorkload> builder)
    {
        builder
            .HasNoKey()
            .ToView("vw_DoctorWorkload", "hr");

        builder.Property(e => e.DepartmentName)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.DoctorName)
            .HasMaxLength(101)
            .IsUnicode(false);
        builder.Property(e => e.Specialization)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}
