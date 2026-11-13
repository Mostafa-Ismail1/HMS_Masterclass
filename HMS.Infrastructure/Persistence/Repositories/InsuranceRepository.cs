using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Insurances;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class InsuranceRepository(ApplicationDbContext context) : IInsuranceRepository
{
    public async Task<int> AddProviderAsync(InsuranceProvider provider, CancellationToken cancellationToken = default)
    {
        await context.InsuranceProviders.AddAsync(provider, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return provider.Id;
    }

    public async Task<IReadOnlyList<InsuranceProviderDto>> GetAllProvidersAsync(CancellationToken cancellationToken = default)
    {
        return await context.InsuranceProviders
            .AsNoTracking()
            .OrderBy(p => p.ProviderName)
            .Select(p => new InsuranceProviderDto(p.Id, p.ProviderName, p.ContactPhone))
            .ToListAsync(cancellationToken);
    }
}
