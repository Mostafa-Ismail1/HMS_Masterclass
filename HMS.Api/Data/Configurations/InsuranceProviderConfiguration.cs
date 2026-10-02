using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Api.Data.Configurations;

public class InsuranceProviderConfiguration : IEntityTypeConfiguration<InsuranceProvider>
{
    public void Configure(EntityTypeBuilder<InsuranceProvider> builder)
    {
        builder.HasKey(e => e.ProviderId).HasName("PK__Insuranc__B54C689DD6937BB9");

        builder.ToTable("InsuranceProviders", "billing");

        builder.HasIndex(e => e.ProviderName, "UQ__Insuranc__7D057CE57B49CCD0").IsUnique();

        builder.Property(e => e.ProviderId).HasColumnName("ProviderID");
        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20)
            .IsUnicode(false);
        builder.Property(e => e.ProviderName)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}
