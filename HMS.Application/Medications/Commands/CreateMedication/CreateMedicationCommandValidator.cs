using FluentValidation;

namespace HMS.Application.Medications.Commands.CreateMedication;

public class CreateMedicationCommandValidator : AbstractValidator<CreateMedicationCommand>
{
    public CreateMedicationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Medication name is required.");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero.");
    }
}
