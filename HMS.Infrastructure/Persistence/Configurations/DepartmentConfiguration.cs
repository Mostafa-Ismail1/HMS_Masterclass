using HMS.Domain.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments", "hr");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DepartmentID");

        builder.Property(e => e.DepartmentName)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.Location)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Ignore(e => e.DomainEvents);
    }
}
