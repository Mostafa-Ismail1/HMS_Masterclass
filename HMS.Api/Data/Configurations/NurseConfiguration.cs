using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
{
    public void Configure(EntityTypeBuilder<Nurse> builder)
    {
        builder.HasKey(e => e.NurseId).HasName("PK__Nurses__43847869AB73EFFE");

        builder.ToTable("Nurses", "hr");

        builder.Property(e => e.NurseId)
            .ValueGeneratedNever()
            .HasColumnName("NurseID");
        builder.Property(e => e.ShiftType)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.HasOne(d => d.NurseNavigation).WithOne(p => p.Nurse)
            .HasForeignKey<Nurse>(d => d.NurseId)
            .HasConstraintName("FK_Nurses_Staff");
    }
}
