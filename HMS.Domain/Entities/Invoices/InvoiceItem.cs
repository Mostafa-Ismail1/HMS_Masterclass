using HMS.Domain.Common;

namespace HMS.Domain.Entities.Invoices;

public class InvoiceItem : Entity<int>
{
    private InvoiceItem(int invoiceId, DateOnly issueDate, string description, decimal amount)
    {
        InvoiceId = invoiceId;
        IssueDate = issueDate;
        Description = description;
        Amount = amount;
    }

    private InvoiceItem()
    {
    }

    public int InvoiceId { get; private set; }
    public DateOnly IssueDate { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Amount { get; private set; }

    public static Result<InvoiceItem> Create(int invoiceId, DateOnly issueDate, string description, decimal amount)
    {
        if (amount < 0)
        {
            return Result.Failure<InvoiceItem>(InvoiceErrors.InvalidAmount);
        }

        return Result.Success(new InvoiceItem(invoiceId, issueDate, description.Trim(), amount));
    }
}
