using HMS.Domain.Entities.LabOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class LabTestConfiguration : IEntityTypeConfiguration<LabTest>
{
    public void Configure(EntityTypeBuilder<LabTest> builder)
    {
        builder.ToTable("LabTests", "clinical");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LabTestID");

        builder.Property(e => e.TestName)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.Cost).HasColumnType("decimal(10, 2)");

        builder.Ignore(e => e.DomainEvents);
    }
}
