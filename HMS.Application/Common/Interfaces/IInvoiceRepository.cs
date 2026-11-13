using HMS.Domain.Entities.Invoices;

namespace HMS.Application.Common.Interfaces;

public record InvoiceItemInputDto(string Description, decimal Amount);
public record InvoiceDto(int InvoiceId, DateOnly IssueDate, int PatientId, int? AdmissionId, decimal TotalAmount, string Status);

public interface IInvoiceRepository
{
    Task<int> AddInvoiceAsync(Invoice invoice, IEnumerable<InvoiceItem> items, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByIdAsync(int id, DateOnly issueDate, CancellationToken cancellationToken = default);
    Task UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InvoiceDto>> GetAllInvoicesAsync(CancellationToken cancellationToken = default);
}
