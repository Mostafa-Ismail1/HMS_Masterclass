using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Prescriptions.Queries.GetPrescriptions;

public record GetPrescriptionsQuery() : IRequest<Result<IReadOnlyList<PrescriptionDto>>>;

public class GetPrescriptionsQueryHandler(IPrescriptionRepository repository)
    : IRequestHandler<GetPrescriptionsQuery, Result<IReadOnlyList<PrescriptionDto>>>
{
    public async Task<Result<IReadOnlyList<PrescriptionDto>>> Handle(GetPrescriptionsQuery request, CancellationToken cancellationToken)
    {
        var prescriptions = await repository.GetAllAsync(cancellationToken);
        return Result.Success(prescriptions);
    }
}
