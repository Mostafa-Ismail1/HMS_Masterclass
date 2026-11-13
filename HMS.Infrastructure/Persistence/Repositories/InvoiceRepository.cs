using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : IInvoiceRepository
{
    public async Task<int> AddInvoiceAsync(Invoice invoice, IEnumerable<InvoiceItem> items, CancellationToken cancellationToken = default)
    {
        await context.Invoices.AddAsync(invoice, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        foreach (var item in items)
        {
            var itemEntity = InvoiceItem.Create(invoice.InvoiceId, invoice.IssueDate, item.Description, item.Amount).Value;
            await context.InvoiceItems.AddAsync(itemEntity, cancellationToken);
        }
        await context.SaveChangesAsync(cancellationToken);

        return invoice.InvoiceId;
    }

    public async Task<Invoice?> GetByIdAsync(int id, DateOnly issueDate, CancellationToken cancellationToken = default)
    {
        return await context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == id && i.IssueDate == issueDate, cancellationToken);
    }

    public async Task UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        context.Invoices.Update(invoice);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<InvoiceDto>> GetAllInvoicesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Invoices
            .AsNoTracking()
            .OrderByDescending(i => i.InvoiceId)
            .Select(i => new InvoiceDto(i.InvoiceId, i.IssueDate, i.PatientId, i.AdmissionId, i.TotalAmount, i.Status))
            .ToListAsync(cancellationToken);
    }
}
