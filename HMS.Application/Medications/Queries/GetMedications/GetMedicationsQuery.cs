using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Medications.Queries.GetMedications;

public record GetMedicationsQuery() : IRequest<Result<IReadOnlyList<MedicationDto>>>;

public class GetMedicationsQueryHandler(IMedicationRepository repository)
    : IRequestHandler<GetMedicationsQuery, Result<IReadOnlyList<MedicationDto>>>
{
    public async Task<Result<IReadOnlyList<MedicationDto>>> Handle(GetMedicationsQuery request, CancellationToken cancellationToken)
    {
        var meds = await repository.GetAllAsync(cancellationToken);
        return Result.Success(meds);
    }
}
