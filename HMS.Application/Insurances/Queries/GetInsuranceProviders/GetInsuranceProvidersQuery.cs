using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Insurances.Queries.GetInsuranceProviders;

public record GetInsuranceProvidersQuery() : IRequest<Result<IReadOnlyList<InsuranceProviderDto>>>;

public class GetInsuranceProvidersQueryHandler(IInsuranceRepository repository)
    : IRequestHandler<GetInsuranceProvidersQuery, Result<IReadOnlyList<InsuranceProviderDto>>>
{
    public async Task<Result<IReadOnlyList<InsuranceProviderDto>>> Handle(GetInsuranceProvidersQuery request, CancellationToken cancellationToken)
    {
        var providers = await repository.GetAllProvidersAsync(cancellationToken);
        return Result.Success(providers);
    }
}
