using FluentValidation;
using HMS.Api.DTOs.Patients;

namespace HMS.Api.Validators.Patients;

public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequestDto>
{
    private static readonly HashSet<string> ValidBloodTypes =
        new(StringComparer.OrdinalIgnoreCase) { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

    public CreatePatientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.Dob)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(dob => dob < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past.")
            .Must(dob => dob > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-130)))
            .WithMessage("Date of birth indicates an age exceeding 130 years.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^01[0125][0-9]{8}$")
            .WithMessage("Phone must be a valid 11-digit Egyptian mobile number (e.g., 01234567890).");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(g => g.Equals("M", StringComparison.OrdinalIgnoreCase) || g.Equals("F", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Gender must be 'M' or 'F'.");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email address is required.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.BloodType), () =>
        {
            RuleFor(x => x.BloodType)
                .Must(b => ValidBloodTypes.Contains(b!))
                .WithMessage("Blood type must be one of: A+, A-, B+, B-, AB+, AB-, O+, O-.");
        });
    }
}
