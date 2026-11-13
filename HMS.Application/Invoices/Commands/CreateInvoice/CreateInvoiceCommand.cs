using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Invoices;
using MediatR;
using FluentValidation;

namespace HMS.Application.Invoices.Commands.CreateInvoice;

public record CreateInvoiceCommand(
    int PatientId,
    int? AdmissionId,
    DateOnly IssueDate,
    List<InvoiceItemInputDto> Items) : IRequest<Result<int>>;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one invoice item is required.");
    }
}

public class CreateInvoiceCommandHandler(IInvoiceRepository repository)
    : IRequestHandler<CreateInvoiceCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var totalAmount = request.Items.Sum(i => i.Amount);
        var invoiceResult = Invoice.Create(request.PatientId, request.AdmissionId, totalAmount, request.IssueDate);
        if (invoiceResult.IsFailure) return Result.Failure<int>(invoiceResult.Error);

        var items = new List<InvoiceItem>();
        foreach (var item in request.Items)
        {
            var itemResult = InvoiceItem.Create(0, request.IssueDate, item.Description, item.Amount);
            if (itemResult.IsFailure) return Result.Failure<int>(itemResult.Error);
            items.Add(itemResult.Value);
        }

        var id = await repository.AddInvoiceAsync(invoiceResult.Value, items, cancellationToken);
        return Result.Success(id);
    }
}
