using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Staff.Queries.GetStaff;

public record GetStaffQuery() : IRequest<Result<IReadOnlyList<StaffDto>>>;

public class GetStaffQueryHandler(IStaffRepository repository)
    : IRequestHandler<GetStaffQuery, Result<IReadOnlyList<StaffDto>>>
{
    public async Task<Result<IReadOnlyList<StaffDto>>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        var staff = await repository.GetAllStaffAsync(cancellationToken);
        return Result.Success(staff);
    }
}
