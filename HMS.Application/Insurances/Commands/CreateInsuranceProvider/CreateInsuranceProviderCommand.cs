using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Insurances;
using MediatR;
using FluentValidation;

namespace HMS.Application.Insurances.Commands.CreateInsuranceProvider;

public record CreateInsuranceProviderCommand(string ProviderName, string ContactPhone) : IRequest<Result<int>>;

public class CreateInsuranceProviderCommandValidator : AbstractValidator<CreateInsuranceProviderCommand>
{
    public CreateInsuranceProviderCommandValidator()
    {
        RuleFor(x => x.ProviderName).NotEmpty().WithMessage("Provider name is required.");
        RuleFor(x => x.ContactPhone).NotEmpty().WithMessage("Contact phone is required.");
    }
}

public class CreateInsuranceProviderCommandHandler(IInsuranceRepository repository)
    : IRequestHandler<CreateInsuranceProviderCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateInsuranceProviderCommand request, CancellationToken cancellationToken)
    {
        var providerResult = InsuranceProvider.Create(request.ProviderName, request.ContactPhone);
        if (providerResult.IsFailure) return Result.Failure<int>(providerResult.Error);

        var id = await repository.AddProviderAsync(providerResult.Value, cancellationToken);
        return Result.Success(id);
    }
}
