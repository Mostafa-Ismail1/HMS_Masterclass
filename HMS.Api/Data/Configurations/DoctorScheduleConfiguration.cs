using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.HasKey(e => new { e.DoctorId, e.DepartmentId, e.DayOfWeek });

        builder.ToTable("DoctorSchedules", "hr");

        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.DayOfWeek)
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.HasOne(d => d.Department).WithMany(p => p.DoctorSchedules)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__DoctorSch__Depar__1DB06A4F");

        builder.HasOne(d => d.Doctor).WithMany(p => p.DoctorSchedules)
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__DoctorSch__Docto__1CBC4616");
    }
}
