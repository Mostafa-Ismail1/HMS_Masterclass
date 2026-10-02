using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class LabTestConfiguration : IEntityTypeConfiguration<LabTest>
{
    public void Configure(EntityTypeBuilder<LabTest> builder)
    {
        builder.HasKey(e => e.LabTestId).HasName("PK__LabTests__64D339C51D216F08");

        builder.ToTable("LabTests", "clinical");

        builder.HasIndex(e => e.TestName, "UQ__LabTests__2AF07A7D2B799BB3").IsUnique();

        builder.Property(e => e.LabTestId).HasColumnName("LabTestID");
        builder.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.TestName)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}
