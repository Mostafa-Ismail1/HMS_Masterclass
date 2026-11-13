using HMS.Domain.Entities.Nurses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
{
    public void Configure(EntityTypeBuilder<Nurse> builder)
    {
        builder.ToTable("Nurses", "hr");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("NurseID");

        builder.Property(e => e.ShiftType)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Ignore(e => e.DomainEvents);
    }
}
