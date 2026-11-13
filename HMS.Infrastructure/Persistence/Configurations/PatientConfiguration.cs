using HMS.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients", "clinical", tb => tb.IsTemporal(ttb =>
        {
            ttb.UseHistoryTable("Patients_History", "clinical");
            ttb.HasPeriodStart("ValidFrom").HasColumnName("ValidFrom");
            ttb.HasPeriodEnd("ValidTo").HasColumnName("ValidTo");
        }));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("PatientID");

        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Dob)
            .HasColumnName("DOB");

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Gender)
            .HasMaxLength(1)
            .IsUnicode(false)
            .HasDefaultValue("M")
            .IsFixedLength();

        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.Address)
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(e => e.BloodType)
            .HasMaxLength(3)
            .IsUnicode(false);

        builder.Ignore(e => e.DomainEvents);
    }
}
