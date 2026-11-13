using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Appointments;
using MediatR;

namespace HMS.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    : IRequestHandler<BookAppointmentCommand, Result<int>>
{
    public async Task<Result<int>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointmentResult = Appointment.Create(
            request.PatientId,
            request.DoctorId,
            request.AppointmentDate,
            request.Reason);

        if (appointmentResult.IsFailure)
        {
            return Result.Failure<int>(appointmentResult.Error);
        }

        var appointmentId = await appointmentRepository.AddAsync(appointmentResult.Value, cancellationToken);

        return Result.Success(appointmentId);
    }
}
