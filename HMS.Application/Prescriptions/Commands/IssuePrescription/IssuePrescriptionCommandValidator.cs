using FluentValidation;

namespace HMS.Application.Prescriptions.Commands.IssuePrescription;

public class IssuePrescriptionCommandValidator : AbstractValidator<IssuePrescriptionCommand>
{
    public IssuePrescriptionCommandValidator()
    {
        RuleFor(x => x.AppointmentId).GreaterThan(0).WithMessage("Appointment ID is required.");
        RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("Doctor ID is required.");
        RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("Patient ID is required.");
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one medication item is required in the prescription.");
    }
}
