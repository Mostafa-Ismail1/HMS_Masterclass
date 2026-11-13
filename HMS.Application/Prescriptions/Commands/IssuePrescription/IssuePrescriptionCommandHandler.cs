using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Prescriptions;
using MediatR;

namespace HMS.Application.Prescriptions.Commands.IssuePrescription;

public class IssuePrescriptionCommandHandler(IPrescriptionRepository repository)
    : IRequestHandler<IssuePrescriptionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(IssuePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var prescriptionResult = Prescription.Create(
            request.AppointmentId,
            request.DoctorId,
            request.PatientId,
            request.DatePrescribed);

        if (prescriptionResult.IsFailure)
        {
            return Result.Failure<int>(prescriptionResult.Error);
        }

        var details = new List<PrescriptionDetail>();
        foreach (var item in request.Items)
        {
            var detailResult = PrescriptionDetail.Create(0, item.MedicationId, item.Dosage, item.Duration);
            if (detailResult.IsFailure)
            {
                return Result.Failure<int>(detailResult.Error);
            }
            details.Add(detailResult.Value);
        }

        var prescriptionId = await repository.AddAsync(prescriptionResult.Value, details, cancellationToken);
        return Result.Success(prescriptionId);
    }
}
