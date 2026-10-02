using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD245E440B");

        builder.ToTable("Departments", "hr");

        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        builder.Property(e => e.DepartmentName)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.Location)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}
