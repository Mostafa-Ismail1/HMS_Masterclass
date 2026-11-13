using HMS.Domain.Entities.LabOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class LabOrderConfiguration : IEntityTypeConfiguration<LabOrder>
{
    public void Configure(EntityTypeBuilder<LabOrder> builder)
    {
        builder.ToTable("LabOrders", "clinical");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LabOrderID");

        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.LabTestId).HasColumnName("LabTestID");

        builder.Property(e => e.OrderDate)
            .HasColumnName("OrderDate")
            .HasDefaultValueSql("(CONVERT([date],getdate()))");

        builder.Property(e => e.ResultDate).HasColumnName("ResultDate");

        builder.Property(e => e.ResultValue)
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasDefaultValue("Pending");

        builder.Ignore(e => e.DomainEvents);
    }
}
