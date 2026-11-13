using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Admissions.Queries.GetAdmissions;

public record GetAdmissionsQuery() : IRequest<Result<IReadOnlyList<AdmissionDto>>>;

public class GetAdmissionsQueryHandler(IAdmissionRepository repository)
    : IRequestHandler<GetAdmissionsQuery, Result<IReadOnlyList<AdmissionDto>>>
{
    public async Task<Result<IReadOnlyList<AdmissionDto>>> Handle(GetAdmissionsQuery request, CancellationToken cancellationToken)
    {
        var admissions = await repository.GetAllAdmissionsAsync(cancellationToken);
        return Result.Success(admissions);
    }
}
