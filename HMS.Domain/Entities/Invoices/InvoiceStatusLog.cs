using HMS.Domain.Common;

namespace HMS.Domain.Entities.Invoices;

public class InvoiceStatusLog : Entity<int>
{
    private InvoiceStatusLog(int invoiceId, string? oldStatus, string? newStatus, DateTime changedAt)
    {
        InvoiceId = invoiceId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedAt = changedAt;
    }

    private InvoiceStatusLog()
    {
    }

    public int InvoiceId { get; private set; }
    public string? OldStatus { get; private set; }
    public string? NewStatus { get; private set; }
    public DateTime? ChangedAt { get; private set; }

    public static Result<InvoiceStatusLog> Create(int invoiceId, string? oldStatus, string? newStatus, DateTime changedAt)
    {
        return Result.Success(new InvoiceStatusLog(invoiceId, oldStatus, newStatus, changedAt));
    }
}
