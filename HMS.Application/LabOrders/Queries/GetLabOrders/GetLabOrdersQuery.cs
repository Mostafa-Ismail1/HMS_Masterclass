using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.LabOrders.Queries.GetLabOrders;

public record GetLabOrdersQuery() : IRequest<Result<IReadOnlyList<LabOrderDto>>>;

public class GetLabOrdersQueryHandler(ILabOrderRepository repository)
    : IRequestHandler<GetLabOrdersQuery, Result<IReadOnlyList<LabOrderDto>>>
{
    public async Task<Result<IReadOnlyList<LabOrderDto>>> Handle(GetLabOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await repository.GetAllOrdersAsync(cancellationToken);
        return Result.Success(orders);
    }
}
