using HMS.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedules", "hr");

        builder.HasKey(e => new { e.DoctorId, e.DepartmentId, e.DayOfWeek });

        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.DayOfWeek)
            .HasMaxLength(10)
            .IsUnicode(false);
    }
}
