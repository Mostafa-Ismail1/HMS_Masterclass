using HMS.Domain.Entities.Insurances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class PatientInsuranceConfiguration : IEntityTypeConfiguration<PatientInsurance>
{
    public void Configure(EntityTypeBuilder<PatientInsurance> builder)
    {
        builder.ToTable("PatientInsurance", "billing");

        builder.HasKey(e => new { e.PatientId, e.ProviderId });

        builder.Property(e => e.PatientId).HasColumnName("PatientID");
        builder.Property(e => e.ProviderId).HasColumnName("ProviderID");

        builder.Property(e => e.PolicyNumber)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.ExpiryDate).HasColumnName("ExpiryDate");
    }
}
