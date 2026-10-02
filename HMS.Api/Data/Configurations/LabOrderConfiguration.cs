using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class LabOrderConfiguration : IEntityTypeConfiguration<LabOrder>
{
    public void Configure(EntityTypeBuilder<LabOrder> builder)
    {
        builder.HasKey(e => e.LabOrderId).HasName("PK__LabOrder__9CBC009E8AC1ED8D");

        builder.ToTable("LabOrders", "clinical");

        builder.HasIndex(e => new { e.DoctorId, e.Status }, "IX_LabOrders_Doctor");

        builder.Property(e => e.LabOrderId).HasColumnName("LabOrderID");
        builder.Property(e => e.DoctorId).HasColumnName("DoctorID");
        builder.Property(e => e.LabTestId).HasColumnName("LabTestID");
        builder.Property(e => e.OrderDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.ResultValue)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.Status)
            .HasMaxLength(15)
            .IsUnicode(false)
            .HasDefaultValue("Pending");

        builder.HasOne(d => d.Doctor).WithMany(p => p.LabOrders)
            .HasForeignKey(d => d.DoctorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Lab_Doctor");

        builder.HasOne(d => d.LabTest).WithMany(p => p.LabOrders)
            .HasForeignKey(d => d.LabTestId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Lab_Test");

        builder.HasOne(d => d.Patient).WithMany(p => p.LabOrders)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Lab_Patient");
    }
}
