using FluentValidation;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Doctors;

namespace HMS.Api.Validators.Doctors;

public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequestDto>
{
    public CreateDoctorRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^01[0125][0-9]{8}$").WithMessage("Phone must be a valid 11-digit Egyptian mobile number (e.g., 01012345678).");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.")
            .MustAsync(async (departmentId, cancellationToken) =>
            {
                return await unitOfWork.Departments.AnyAsync(
                    d => d.DepartmentId == departmentId,
                    cancellationToken);
            })
            .WithMessage("The specified Department does not exist in the hospital.");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required.")
            .Length(3, 100).WithMessage("Specialization must be between 3 and 100 characters.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("Medical license number is required.")
            .Length(5, 50).WithMessage("License number must be between 5 and 50 characters.");

        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(6000).WithMessage("Doctor salary cannot be less than the hospital minimum wage (6,000 EGP).");
    }
}
