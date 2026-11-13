using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Medications;
using MediatR;

namespace HMS.Application.Medications.Commands.CreateMedication;

public class CreateMedicationCommandHandler(IMedicationRepository repository)
    : IRequestHandler<CreateMedicationCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
    {
        var result = Medication.Create(request.Name, request.Manufacturer, request.UnitPrice);
        if (result.IsFailure)
        {
            return Result.Failure<int>(result.Error);
        }

        var id = await repository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }
}
