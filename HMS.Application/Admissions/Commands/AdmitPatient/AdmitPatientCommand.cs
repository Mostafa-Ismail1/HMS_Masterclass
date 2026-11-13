using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Admissions;
using MediatR;
using FluentValidation;

namespace HMS.Application.Admissions.Commands.AdmitPatient;

public record AdmitPatientCommand(int PatientId, int RoomId, int AdmittingDoctorId, DateTime AdmissionDate) : IRequest<Result<int>>;

public class AdmitPatientCommandValidator : AbstractValidator<AdmitPatientCommand>
{
    public AdmitPatientCommandValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.RoomId).GreaterThan(0);
        RuleFor(x => x.AdmittingDoctorId).GreaterThan(0);
    }
}

public class AdmitPatientCommandHandler(IAdmissionRepository repository)
    : IRequestHandler<AdmitPatientCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AdmitPatientCommand request, CancellationToken cancellationToken)
    {
        var admissionResult = Admission.Create(request.PatientId, request.RoomId, request.AdmittingDoctorId, request.AdmissionDate);
        if (admissionResult.IsFailure) return Result.Failure<int>(admissionResult.Error);

        var id = await repository.AddAdmissionAsync(admissionResult.Value, cancellationToken);
        return Result.Success(id);
    }
}
