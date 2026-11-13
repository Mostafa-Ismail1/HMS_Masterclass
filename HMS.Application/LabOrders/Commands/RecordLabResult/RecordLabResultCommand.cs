using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.LabOrders;
using MediatR;
using FluentValidation;

namespace HMS.Application.LabOrders.Commands.RecordLabResult;

public record RecordLabResultCommand(int LabOrderId, string ResultValue, DateOnly ResultDate) : IRequest<Result>;

public class RecordLabResultCommandValidator : AbstractValidator<RecordLabResultCommand>
{
    public RecordLabResultCommandValidator()
    {
        RuleFor(x => x.LabOrderId).GreaterThan(0);
        RuleFor(x => x.ResultValue).NotEmpty().WithMessage("Result value is required.");
    }
}

public class RecordLabResultCommandHandler(ILabOrderRepository repository)
    : IRequestHandler<RecordLabResultCommand, Result>
{
    public async Task<Result> Handle(RecordLabResultCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.LabOrderId, cancellationToken);
        if (order is null) return Result.Failure(LabOrderErrors.NotFound);

        var recordResult = order.RecordResult(request.ResultValue, request.ResultDate);
        if (recordResult.IsFailure) return recordResult;

        await repository.UpdateOrderAsync(order, cancellationToken);
        return Result.Success();
    }
}
