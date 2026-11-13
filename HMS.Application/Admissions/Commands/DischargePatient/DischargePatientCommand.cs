using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Admissions;
using MediatR;
using FluentValidation;

namespace HMS.Application.Admissions.Commands.DischargePatient;

public record DischargePatientCommand(int AdmissionId, DateTime DischargeDate) : IRequest<Result>;

public class DischargePatientCommandValidator : AbstractValidator<DischargePatientCommand>
{
    public DischargePatientCommandValidator()
    {
        RuleFor(x => x.AdmissionId).GreaterThan(0);
    }
}

public class DischargePatientCommandHandler(IAdmissionRepository repository)
    : IRequestHandler<DischargePatientCommand, Result>
{
    public async Task<Result> Handle(DischargePatientCommand request, CancellationToken cancellationToken)
    {
        var admission = await repository.GetByIdAsync(request.AdmissionId, cancellationToken);
        if (admission is null) return Result.Failure(AdmissionErrors.NotFound);

        var dischargeResult = admission.Discharge(request.DischargeDate);
        if (dischargeResult.IsFailure) return dischargeResult;

        await repository.UpdateAdmissionAsync(admission, cancellationToken);
        return Result.Success();
    }
}
