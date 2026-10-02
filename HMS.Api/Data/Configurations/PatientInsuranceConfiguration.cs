using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class PatientInsuranceConfiguration : IEntityTypeConfiguration<PatientInsurance>
{
    public void Configure(EntityTypeBuilder<PatientInsurance> builder)
    {
        builder.HasKey(e => new { e.PatientId, e.ProviderId });

        builder.ToTable("PatientInsurance", "billing");

        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.ProviderId).HasColumnName("ProviderID");
        builder.Property(e => e.PolicyNumber)
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.HasOne(d => d.Patient).WithMany(p => p.PatientInsurances)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__PatientIn__Patie__0C85DE4D");

        builder.HasOne(d => d.Provider).WithMany(p => p.PatientInsurances)
            .HasForeignKey(d => d.ProviderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__PatientIn__Provi__0D7A0286");
    }
}
