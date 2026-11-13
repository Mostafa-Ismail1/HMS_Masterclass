using HMS.Domain.Entities.Insurances;

namespace HMS.Application.Common.Interfaces;

public record InsuranceProviderDto(int ProviderId, string ProviderName, string ContactPhone);

public interface IInsuranceRepository
{
    Task<int> AddProviderAsync(InsuranceProvider provider, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InsuranceProviderDto>> GetAllProvidersAsync(CancellationToken cancellationToken = default);
}
