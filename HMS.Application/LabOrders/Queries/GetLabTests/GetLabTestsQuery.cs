using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.LabOrders.Queries.GetLabTests;

public record GetLabTestsQuery() : IRequest<Result<IReadOnlyList<LabTestDto>>>;

public class GetLabTestsQueryHandler(ILabOrderRepository repository)
    : IRequestHandler<GetLabTestsQuery, Result<IReadOnlyList<LabTestDto>>>
{
    public async Task<Result<IReadOnlyList<LabTestDto>>> Handle(GetLabTestsQuery request, CancellationToken cancellationToken)
    {
        var tests = await repository.GetAllTestsAsync(cancellationToken);
        return Result.Success(tests);
    }
}
