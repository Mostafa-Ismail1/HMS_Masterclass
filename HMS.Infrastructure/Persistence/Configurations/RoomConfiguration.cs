using HMS.Domain.Entities.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms", "hr");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("RoomID");

        builder.Property(e => e.RoomNumber)
            .HasMaxLength(10)
            .IsUnicode(false);

        builder.Property(e => e.RoomType)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.DailyRate).HasColumnType("decimal(10, 2)");

        builder.Ignore(e => e.DomainEvents);
    }
}
