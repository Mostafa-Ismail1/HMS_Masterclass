using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.LabOrders;
using MediatR;
using FluentValidation;

namespace HMS.Application.LabOrders.Commands.CreateLabOrder;

public record CreateLabOrderCommand(int PatientId, int DoctorId, int LabTestId, DateOnly OrderDate) : IRequest<Result<int>>;

public class CreateLabOrderCommandValidator : AbstractValidator<CreateLabOrderCommand>
{
    public CreateLabOrderCommandValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.LabTestId).GreaterThan(0);
    }
}

public class CreateLabOrderCommandHandler(ILabOrderRepository repository)
    : IRequestHandler<CreateLabOrderCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateLabOrderCommand request, CancellationToken cancellationToken)
    {
        var orderResult = LabOrder.Create(request.PatientId, request.DoctorId, request.LabTestId, request.OrderDate);
        if (orderResult.IsFailure) return Result.Failure<int>(orderResult.Error);

        var id = await repository.AddOrderAsync(orderResult.Value, cancellationToken);
        return Result.Success(id);
    }
}
