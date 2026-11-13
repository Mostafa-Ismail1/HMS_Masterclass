using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Admissions.Queries.GetRooms;

public record GetRoomsQuery() : IRequest<Result<IReadOnlyList<RoomDto>>>;

public class GetRoomsQueryHandler(IAdmissionRepository repository)
    : IRequestHandler<GetRoomsQuery, Result<IReadOnlyList<RoomDto>>>
{
    public async Task<Result<IReadOnlyList<RoomDto>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await repository.GetAllRoomsAsync(cancellationToken);
        return Result.Success(rooms);
    }
}
