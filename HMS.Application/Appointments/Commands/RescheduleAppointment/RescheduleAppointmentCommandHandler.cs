using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Appointments;
using MediatR;

namespace HMS.Application.Appointments.Commands.RescheduleAppointment;

public class RescheduleAppointmentCommandHandler(IAppointmentRepository repository)
    : IRequestHandler<RescheduleAppointmentCommand, Result>
{
    public async Task<Result> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await repository.GetByIdAsync(request.AppointmentId, cancellationToken);
        if (appointment is null)
        {
            return Result.Failure(AppointmentErrors.NotFound);
        }

        var result = appointment.Reschedule(request.NewAppointmentDate);
        if (result.IsFailure)
        {
            return result;
        }

        await repository.UpdateAsync(appointment, cancellationToken);
        return Result.Success();
    }
}
