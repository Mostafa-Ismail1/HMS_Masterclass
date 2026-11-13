using HMS.Domain.Entities.Insurances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure.Persistence.Configurations;

public class InsuranceProviderConfiguration : IEntityTypeConfiguration<InsuranceProvider>
{
    public void Configure(EntityTypeBuilder<InsuranceProvider> builder)
    {
        builder.ToTable("InsuranceProviders", "billing");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ProviderID");

        builder.Property(e => e.ProviderName)
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.ContactPhone)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Ignore(e => e.DomainEvents);
    }
}
