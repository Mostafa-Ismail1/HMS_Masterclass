using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Prescriptions.Commands.IssuePrescription;

public record IssuePrescriptionCommand(
    int AppointmentId,
    int DoctorId,
    int PatientId,
    DateOnly DatePrescribed,
    List<PrescriptionItemDto> Items) : IRequest<Result<int>>;
