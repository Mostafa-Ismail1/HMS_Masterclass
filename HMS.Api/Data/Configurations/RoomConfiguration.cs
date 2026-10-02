using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(e => e.RoomId).HasName("PK__Rooms__328639190A5B149A");

        builder.ToTable("Rooms", "hr");

        builder.HasIndex(e => e.RoomNumber, "UQ__Rooms__AE10E07A21D8398C").IsUnique();

        builder.Property(e => e.RoomId).HasColumnName("RoomID");
        builder.Property(e => e.DailyRate).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.RoomNumber)
            .HasMaxLength(10)
            .IsUnicode(false);
        builder.Property(e => e.RoomType)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.HasOne(d => d.Department).WithMany(p => p.Rooms)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Rooms_Department");
    }
}
