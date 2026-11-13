using HMS.Domain.Entities.Staff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff", "hr");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("StaffID");

        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.HireDate)
            .HasColumnName("HireDate")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

        builder.Property(e => e.JobTitle)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

        builder.Ignore(e => e.DomainEvents);
    }
}
