using FluentValidation;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Departments;

namespace HMS.Api.Validators.Departments;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequestDto>
{
    public CreateDepartmentRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty()
            .WithMessage("Department name is mandatory and cannot be empty.")
            .MinimumLength(3)
            .WithMessage("Department name must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Department name cannot exceed 100 characters.")
            .MustAsync(async (name, cancellationToken) =>
            {
                var exists = await unitOfWork.Departments.AnyAsync(
                    d => d.DepartmentName.ToLower() == name.Trim().ToLower(),
                    cancellationToken);
                return !exists;
            })
            .WithMessage("A department with this name already exists in the hospital.");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .WithMessage("Location cannot exceed 200 characters.");
    }
}
