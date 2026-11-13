using FluentValidation;

namespace HMS.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithErrorCode("CancelAppointment.InvalidId")
            .WithMessage("Appointment ID must be greater than zero.");
    }
}
