using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Invoices;
using MediatR;
using FluentValidation;

namespace HMS.Application.Invoices.Commands.PayInvoice;

public record PayInvoiceCommand(int InvoiceId, DateOnly IssueDate) : IRequest<Result>;

public class PayInvoiceCommandValidator : AbstractValidator<PayInvoiceCommand>
{
    public PayInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId).GreaterThan(0);
    }
}

public class PayInvoiceCommandHandler(IInvoiceRepository repository)
    : IRequestHandler<PayInvoiceCommand, Result>
{
    public async Task<Result> Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.InvoiceId, request.IssueDate, cancellationToken);
        if (invoice is null) return Result.Failure(InvoiceErrors.NotFound);

        var payResult = invoice.MarkAsPaid();
        if (payResult.IsFailure) return payResult;

        await repository.UpdateInvoiceAsync(invoice, cancellationToken);
        return Result.Success();
    }
}
