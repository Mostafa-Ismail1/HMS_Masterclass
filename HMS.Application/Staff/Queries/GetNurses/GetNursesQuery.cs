using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Staff.Queries.GetNurses;

public record GetNursesQuery() : IRequest<Result<IReadOnlyList<NurseDto>>>;

public class GetNursesQueryHandler(IStaffRepository repository)
    : IRequestHandler<GetNursesQuery, Result<IReadOnlyList<NurseDto>>>
{
    public async Task<Result<IReadOnlyList<NurseDto>>> Handle(GetNursesQuery request, CancellationToken cancellationToken)
    {
        var nurses = await repository.GetAllNursesAsync(cancellationToken);
        return Result.Success(nurses);
    }
}
