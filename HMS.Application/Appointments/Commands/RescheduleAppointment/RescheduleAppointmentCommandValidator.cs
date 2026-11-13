using FluentValidation;

namespace HMS.Application.Appointments.Commands.RescheduleAppointment;

public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
{
    public RescheduleAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithErrorCode("RescheduleAppointment.InvalidId")
            .WithMessage("Appointment ID must be greater than zero.");

        RuleFor(x => x.NewAppointmentDate)
            .NotEmpty()
            .WithErrorCode("RescheduleAppointment.InvalidDate")
            .WithMessage("New appointment date is required.");
    }
}
