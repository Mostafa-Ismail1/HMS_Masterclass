using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF70D7F424A");

        builder.ToTable("Staff", "hr");

        builder.HasIndex(e => e.Email, "UQ__Staff__A9D105342096F3E6").IsUnique();

        builder.Property(e => e.StaffId).HasColumnName("StaffID");
        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
        builder.Property(e => e.JobTitle)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false);
        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsUnicode(false);
        builder.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

        builder.HasOne(d => d.Department).WithMany(p => p.Staff)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Staff_Department");
    }
}
