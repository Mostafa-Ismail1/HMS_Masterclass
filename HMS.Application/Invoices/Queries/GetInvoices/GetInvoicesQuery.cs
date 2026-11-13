using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Invoices.Queries.GetInvoices;

public record GetInvoicesQuery() : IRequest<Result<IReadOnlyList<InvoiceDto>>>;

public class GetInvoicesQueryHandler(IInvoiceRepository repository)
    : IRequestHandler<GetInvoicesQuery, Result<IReadOnlyList<InvoiceDto>>>
{
    public async Task<Result<IReadOnlyList<InvoiceDto>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await repository.GetAllInvoicesAsync(cancellationToken);
        return Result.Success(invoices);
    }
}
