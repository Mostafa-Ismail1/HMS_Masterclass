using FluentValidation;

namespace HMS.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentCommandValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithErrorCode("BookAppointment.InvalidPatient")
            .WithMessage("Patient ID must be greater than zero.");

        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithErrorCode("BookAppointment.InvalidDoctor")
            .WithMessage("Doctor ID must be greater than zero.");

        RuleFor(x => x.Reason)
            .MaximumLength(200)
            .WithErrorCode("BookAppointment.ReasonTooLong")
            .WithMessage("Reason cannot exceed 200 characters.");
    }
}
