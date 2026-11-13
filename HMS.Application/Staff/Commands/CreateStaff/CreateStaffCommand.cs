using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Staff;
using MediatR;
using FluentValidation;

namespace HMS.Application.Staff.Commands.CreateStaff;

public record CreateStaffCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateOnly HireDate,
    int DepartmentId,
    string JobTitle,
    decimal Salary) : IRequest<Result<int>>;

public class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.Salary).GreaterThan(0);
    }
}

public class CreateStaffCommandHandler(IStaffRepository repository)
    : IRequestHandler<CreateStaffCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var staffResult = Domain.Entities.Staff.Staff.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.HireDate,
            request.DepartmentId,
            request.JobTitle,
            request.Salary);

        if (staffResult.IsFailure) return Result.Failure<int>(staffResult.Error);

        var id = await repository.AddStaffAsync(staffResult.Value, cancellationToken);
        return Result.Success(id);
    }
}
